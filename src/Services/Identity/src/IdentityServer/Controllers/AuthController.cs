using AutoMapper;
using EventBus.Events;
using DataAccess.Models.Response;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using IdentityServer.Services;
using System.Net;
using IdentityServer.Models.DTOs.Request;
using IdentityServer.Models.DTOs.Response;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly MessageService _messageService;

        private readonly IMassTransitService _massTransitService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMassTransitService massTransitService, IJwtService jwtService, RoleManager<IdentityRole> roleManager, IMapper mapper, ApplicationDbContext context, MessageService messageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _massTransitService = massTransitService;
            _jwtService = jwtService;
            _roleManager = roleManager;
            _mapper = mapper;
            _context = context;
            _messageService = messageService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                //return NotFound(new ResponseObject<string>(System.Net.HttpStatusCode.NotFound, "User does not exist!"));
                return NotFound(ResponseObject.Failure(error: "User does not exist!"));

            if (!user.EmailConfirmed)
                return BadRequest(ResponseObject.Failure(error: "Email not confirmed!"));

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized(ResponseObject.Failure(error: "Invalid credentials!"));

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(ResponseObject.Success(data: new TokenResponse { AccessToken = await GenerateAccessToken(user), RefreshToken = await GenerateRefreshToken(user) }, message: "Login success!"));
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            string baseUrl = "api-gateway.hdang09.me";

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var roleExist = await _roleManager.RoleExistsAsync(model.RoleName);
                    //var roleExist = await _roleManager.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Name.Equals(model.RoleName));
                    if (!roleExist)
                    {
                        return BadRequest(ResponseObject.Failure(error: "Role does not exist!"));
                    }

                    var userResult = await _userManager.CreateAsync(user, model.Password);
                    if (!userResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(ResponseObject.Failure(data: userResult.Errors.FirstOrDefault().Description, error: "User create failed!"));
                    }

                    var roleResult = await _userManager.AddToRoleAsync(user, model.RoleName);
                    if (!roleResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(ResponseObject.Failure(data: userResult.Errors.FirstOrDefault().Description, error: "Role assign failed!"));
                    }

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = $"https://localhost:5001/api/Auth/ConfirmEmail?userId={user.Id}&code={code}";
                    var callbackUrl = $"https://{baseUrl}/identityserver/api/Auth/ConfirmEmail?userId={user.Id}&code={Uri.EscapeDataString(code)}";
                    //var callbackUrl = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);

                    await _messageService.SendEmailAsync(user.Email, "Confirm Your Account",
                        $@"<div style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                            <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 5px;'>
                                <h2 style='color: #333333; text-align: center;'>Welcome to Eco-Clothes!</h2>
                                <p style='color: #666666; font-size: 16px;'>
                                    Hi {user.UserName},<br/><br/>
                                    Thanks for signing up! Please confirm your email address by clicking the button below.
                                </p>
                                <div style='text-align: center; margin: 20px 0;'>
                                    <a href='{callbackUrl}' style='background-color: #4CAF50; color: #ffffff; padding: 12px 20px; border-radius: 5px; text-decoration: none; font-weight: bold; display: inline-block;'>
                                        Confirm Your Account
                                    </a>
                                </div>
                                <p style='color: #666666; font-size: 16px;'>
                                    Or, copy and paste the following URL into your browser:
                                </p>
                                <p style='color: #333333; font-size: 14px; word-break: break-all;'>
                                    <a href='{callbackUrl}' style='color: #4CAF50;'>{callbackUrl}</a>
                                </p>
                                <hr style='border: none; border-top: 1px solid #dddddd; margin: 20px 0;' />
                                <p style='color: #999999; font-size: 12px; text-align: center;'>
                                    If you did not sign up for this account, you can ignore this email. <br/>
                                    Thank you! <br/><br/>
                                    Eco-Clothes
                                </p>
                            </div>
                        </div>"
                        );

                    await _massTransitService.Publish(new UserCreatedEvent
                    {
                        UserId = Guid.Parse(user.Id),
                        Email = user.Email,
                        FullName = user.FullName,
                        ImgUrl = user.ImgUrl,
                        Phone = user.PhoneNumber,
                        Role = model.RoleName,
                        Password = user.PasswordHash,
                    });

                    await transaction.CommitAsync();
                    return Ok(ResponseObject.Success(code: HttpStatusCode.Created, data: new TokenResponse { AccessToken = await GenerateAccessToken(user), RefreshToken = await GenerateRefreshToken(user) }, message: "Please check your email to confirm email!"));

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(ResponseObject.Failure(error: ex.Message));
                }
            }
        }

        //[HttpGet("Logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();
        //    return Ok(ResponseObject.Failure("User logged out!"));
        //}

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest(ResponseObject.Failure(error: "Invalid request!"));
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest(ResponseObject.Failure(error: "User does not exist!"));
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded ? Ok(ResponseObject.Success(message: "Confirm Email successfull!")) : BadRequest(ResponseObject.Failure(data: result.Errors, error: "Confirm Email failed!"));
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return BadRequest(ResponseObject.Failure(error: "Invalid Email Address!"));
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            //var callbackUrl = Url.Action("ResetPassword", "Auth", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

            await _massTransitService.Publish(new UserPasswordResetOccurredEvent
            {
                Email = user.Email,
                Code = code
            });

            return Ok(ResponseObject.Success(message: "Login success!"));
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(ResponseObject.Failure(error: "User does not exist!"));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(ResponseObject.Failure(error: "Invalid request!"));
            }
            return Ok(ResponseObject.Success(message: "Password reset successfull!"));
        }

        [Authorize]
        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] GetTokenDTO model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(ResponseObject.Failure(error: "Unauthorized!"));

            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token.Equals(model.RefreshToken));
            if (refreshToken is null)
                return NotFound(ResponseObject.Failure(error: "Token does not exist!"));

            if (!refreshToken.UserId.Equals(user.Id))
                return NotFound(ResponseObject.Failure(error: "Token invalid!"));

            if (refreshToken.ExpiryDate < DateTime.UtcNow)
            {
                return Ok(ResponseObject.Success(data: new TokenResponse { AccessToken = await GenerateAccessToken(user), RefreshToken = await GenerateRefreshToken(user) }, message: "Get token success!"));
            }

            var accessToken = await GenerateAccessToken(user);

            return Ok(ResponseObject.Success(data: new { accessToken, refreshToken.Token }, message: "Get token success!"));
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized(ResponseObject.Failure(code: HttpStatusCode.Unauthorized, error: "Invalid request!"));

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded) return Ok(ResponseObject.Success(code: HttpStatusCode.OK, message: "Password has been changed successfully"));

            return BadRequest(ResponseObject.Failure(error: "Invalid request!"));
        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<IActionResult> CurrentUser()
        {
            var user = await _userManager.FindByIdAsync(User.Identity?.Name!);
            //var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized(ResponseObject.Failure(code: HttpStatusCode.Unauthorized, error: "Invalid request!"));

            var userDto = _mapper.Map<CurrentUserDTO>(user);
            return Ok(ResponseObject.Success(data: userDto);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleDTO model)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole { Name = model.RoleName });
            if (result.Succeeded)
            {
                return Ok(ResponseObject.Success(code: HttpStatusCode.Created, message: "Role create successfull!"));
            }

            return BadRequest(ResponseObject.Failure(error: result.Errors.FirstOrDefault().Description));
        }

        [HttpGet("LoginGoogle")]
        public IActionResult LoginGoogle(string returnUrl = "/")
        {
            var redirectUrl = Url.Action("GoogleResponse", "Auth", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("GoogleResponse")]
        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            try
            {
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return BadRequest(ResponseObject.Failure(error: "Login failed!"));
                }

                var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
                if (result.Succeeded)
                {
                    return Ok(ResponseObject.Success(message: "Login success!"));
                }

                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var name = info.Principal.FindFirstValue(ClaimTypes.Name);
                var user = new ApplicationUser
                {
                    FullName = name,
                    Email = email,
                };

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var userResult = await _userManager.CreateAsync(user, "Password123!");
                    if (!userResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(ResponseObject.Failure(error: userResult.Errors.FirstOrDefault().Description, data: "User create failed!"));
                    }

                    var roleResult = await _userManager.AddToRoleAsync(user, "USER");
                    if (roleResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(ResponseObject.Failure(error: userResult.Errors.FirstOrDefault().Description, data: "Role assign failed!"));
                    }

                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(ResponseObject.Success(message: "User created and logged in successfully"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseObject.Failure(ex.Message, "Internal Server!"));
            }
        }

        private async Task<string> GenerateRefreshToken(ApplicationUser user)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId.Equals(user.Id));

            if (refreshToken == null)
            {
                refreshToken = RefreshToken.Create(user.Id, 1);
                _context.RefreshTokens.Add(refreshToken);
                await _context.SaveChangesAsync();
            }
            else if (refreshToken.ExpiryDate < DateTime.UtcNow)
            {
                refreshToken.ExpiryDate = DateTime.UtcNow.AddDays(30);
                refreshToken.Token = RefreshToken.GenerateToken();

                _context.RefreshTokens.Update(refreshToken);
                await _context.SaveChangesAsync();
            }

            return refreshToken.Token;
        }

        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var claimsIdentity = new List<Claim>
            {
                new Claim("sub", user.Id)
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            claimsIdentity.Add(new Claim("role", userRoles.FirstOrDefault()));

            var jwtToken = _jwtService.GenerateJwtToken(claimsIdentity);

            return jwtToken;
        }
    }
}
