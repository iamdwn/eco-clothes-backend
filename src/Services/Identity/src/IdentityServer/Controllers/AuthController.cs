using AutoMapper;
using EventBus.Events;
using EventBus.Response;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer.Models.DTOs;
using IdentityServer.Models.Response;
using IdentityServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

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

        private readonly IMassTransitService _massTransitService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMassTransitService massTransitService, IJwtService jwtService, RoleManager<IdentityRole> roleManager, IMapper mapper, ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _massTransitService = massTransitService;
            _jwtService = jwtService;
            _roleManager = roleManager;
            _mapper = mapper;
            _context = context;
            _currentUserService = currentUserService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                //return NotFound(new ResponseObject<string>(System.Net.HttpStatusCode.NotFound, "User does not exist!"));
                return NotFound(ResponseObject.Failure("User does not exist!"));

            if (!user.EmailConfirmed)
                return BadRequest(ResponseObject.Failure("Email not confirmed!"));

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized(ResponseObject.Failure("Invalid credentials!"));

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(ResponseObject.Success<object>(new TokenResponse { AccessToken = await GenerateAccessToken(user), RefreshToken = await GenerateRefreshToken(user) }, "Login success!"));
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var user = _mapper.Map<ApplicationUser>(model);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var roleExist = await _roleManager.RoleExistsAsync(model.RoleName);
                    //var roleExist = await _roleManager.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Name.Equals(model.RoleName));
                    if (!roleExist)
                    {
                        return BadRequest(ResponseObject.Failure("Role does not exist!"));
                    }

                    var userResult = await _userManager.CreateAsync(user, model.Password);
                    if (!userResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(ResponseObject.Failure<object>(userResult.Errors.FirstOrDefault().Description, "User does not exist!"));
                    }

                    var roleResult = await _userManager.AddToRoleAsync(user, model.RoleName);
                    if (!roleResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(ResponseObject.Failure<object>(userResult.Errors.FirstOrDefault().Description, "Role assign failed!"));
                    }

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                    await _massTransitService.Publish(new UserCreatedEvent
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        CallbackUrl = callbackUrl,
                    });

                    await transaction.CommitAsync();
                    return Ok(ResponseObject.Success("User create account with password!"));

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(ResponseObject.Failure(ex.Message));
                }
            }
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(ResponseObject.Failure("User logged out!"));
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDTO model)
        {
            if (model.UserId == null || model.Code == null)
            {
                return BadRequest(ResponseObject.Failure<string>("Invalid request!"));
            }

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return BadRequest(ResponseObject.Failure<string>("User does not exist!"));
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.Code);
            return result.Succeeded ? Ok(ResponseObject.Success("Confirm Email successfull!")) : BadRequest(ResponseObject.Failure<object>(result.Errors));
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return BadRequest(ResponseObject.Failure("Invalid Email Address!"));
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            //var callbackUrl = Url.Action("ResetPassword", "Auth", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

            await _massTransitService.Publish(new UserPasswordResetOccurredEvent
            {
                Email = user.Email,
                Code = code
            });

            return Ok(ResponseObject.Success("Login success!"));
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(ResponseObject.Failure("User does not exist!"));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok(ResponseObject.Success("Password reset successfull!"));
            }

            return BadRequest(ResponseObject.Failure("Invalid request!"));
        }

        [Authorize]
        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] GetTokenDTO model)
        {
            var user = await _userManager.GetUserAsync(User);
            var testUser = _currentUserService.UserId;
            if (user == null)
                return Unauthorized(ResponseObject.Failure("Unauthorized!"));

            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token.Equals(model.RefreshToken));
            if (refreshToken is null)
                return NotFound(ResponseObject.Failure("Token does not exist!"));

            if (!refreshToken.UserId.Equals(user.Id))
                return NotFound(ResponseObject.Failure("Token invalid!"));

            if (refreshToken.ExpiryDate < DateTime.UtcNow)
            {
                return Ok(ResponseObject.Success<object>(new TokenResponse { AccessToken = await GenerateAccessToken(user), RefreshToken = await GenerateRefreshToken(user) }, "Get token success!"));
            }

            var accessToken = await GenerateAccessToken(user);

            return Ok(ResponseObject.Success<object>(new { accessToken, refreshToken.Token }));
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized(ResponseObject.Failure("Invalid request!"));

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded) return Ok(ResponseObject.Success("Password has been changed successfully"));

            return BadRequest(ResponseObject.Failure("Invalid request!"));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleDTO model)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole { Name = model.RoleName });
            if (result.Succeeded)
            {
                return Ok(ResponseObject.Success("Role create successfull!"));
            }

            return BadRequest(ResponseObject.Failure(result.Errors.FirstOrDefault().Description));
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
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            claimsIdentity.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, userRoles.FirstOrDefault()));

            var jwtToken = _jwtService.GenerateJwtToken(claimsIdentity);

            return jwtToken;
        }
    }
}
