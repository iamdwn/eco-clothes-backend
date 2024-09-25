using AutoMapper;
using EventBus.Events;
using EventBus.Response;
using IdentityServer.Data;
using IdentityServer.Models;
using IdentityServer.Models.DTOs;
using IdentityServer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMassTransitService massTransitService, IJwtService jwtService, RoleManager<IdentityRole> roleManager, IMapper mapper, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _massTransitService = massTransitService;
            _jwtService = jwtService;
            _roleManager = roleManager;
            _mapper = mapper;
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return NotFound(new ResponseObject<string>(System.Net.HttpStatusCode.NotFound, "User does not exist!"));

            if (!user.EmailConfirmed) return BadRequest(new ResponseObject<string>(System.Net.HttpStatusCode.BadRequest, "Email not confirmed!"));

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized(new ResponseObject<string>(System.Net.HttpStatusCode.BadRequest, "Invalid credentials!"));

            await _signInManager.SignInAsync(user, isPersistent: false);

            var claims = await _userManager.GetClaimsAsync(user);
            var refreshTokenClaim = claims.FirstOrDefault(c => c.Type == "refreshToken");
            string refreshToken;

            if (refreshTokenClaim == null || !_jwtService.ValidateToken(refreshTokenClaim.Value, user))
            {
                refreshToken = _jwtService.GenerateRefreshToken(user);

                if (refreshTokenClaim == null)
                    await _userManager.AddClaimAsync(user, new Claim("refreshToken", refreshToken));
                else
                    await _userManager.ReplaceClaimAsync(user, refreshTokenClaim, new Claim("refreshToken", refreshToken));
            }
            else
            {
                refreshToken = refreshTokenClaim.Value;
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtService.GenerateToken(user, userRoles.FirstOrDefault() ?? "");

            return Ok(new ResponseObject<object>
            {
                Data = new
                {
                    access_token = accessToken,
                    refresh_token = refreshToken,
                },
                Message = "Login success!",
                Status = System.Net.HttpStatusCode.OK
            });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            var user = _mapper.Map<ApplicationUser>(model);

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var roleExist = await _roleManager.RoleExistsAsync(model.RoleName);

                    if (!roleExist)
                    {
                        return BadRequest(new ResponseObject<string>(System.Net.HttpStatusCode.BadRequest, "Role does not exist!"));
                    }

                    var userResult = await _userManager.CreateAsync(user, model.Password);

                    if (!userResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(new ResponseObject<object>(userResult.Errors, System.Net.HttpStatusCode.BadRequest, "User create failed!"));
                    }

                    var roleResult = await _userManager.AddToRoleAsync(user, "User");

                    if (!roleResult.Succeeded)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(new ResponseObject<object>(roleResult.Errors, System.Net.HttpStatusCode.BadRequest, "Role assign failed!"));
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
                    return Ok(new ResponseObject<string>(System.Net.HttpStatusCode.Created, "User create successfull!"));

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new ResponseObject<string>(ex.Message, System.Net.HttpStatusCode.BadRequest, ex.Message));
                }
            }
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new ResponseObject<string>(System.Net.HttpStatusCode.OK, "User logged out!"));
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDTO model)
        {
            if (model.UserId == null || model.Code == null)
            {
                return BadRequest(new ResponseObject<string>(System.Net.HttpStatusCode.BadRequest, "Invalid request!"));
            }

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return BadRequest(new ResponseObject<string>(System.Net.HttpStatusCode.BadRequest, "User does not exist!"));
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.Code);
            return result.Succeeded ? Ok(new ResponseObject<string>(System.Net.HttpStatusCode.OK, "Confirm Email successfull!")) : BadRequest(new ResponseObject<object>(result.Errors, System.Net.HttpStatusCode.BadRequest, "Error!"));
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return BadRequest(new ResponseObject<string>(System.Net.HttpStatusCode.BadRequest, "Invalid Email Address!"));
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            //var callbackUrl = Url.Action("ResetPassword", "Auth", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

            await _massTransitService.Publish(new UserPasswordResetOccurredEvent
            {
                Email = user.Email,
                Code = code
            });

            return Ok(new ResponseObject<object>(code, System.Net.HttpStatusCode.OK, "Login success!"));
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new ResponseObject<string>(System.Net.HttpStatusCode.BadRequest, "User does not exist!"));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok(new ResponseObject<string>(System.Net.HttpStatusCode.OK, "Password reset successfull!"));
            }

            return BadRequest(new ResponseObject<string>(System.Net.HttpStatusCode.BadRequest, "Invalid request!"));
        }

        [Authorize]
        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken(GetTokenDTO model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null) return NotFound(new ResponseObject<string>(System.Net.HttpStatusCode.NotFound, "User does not exist!"));

            var isTokenValid = _jwtService.ValidateToken(model.Token, user);

            if (!isTokenValid) return BadRequest(new ResponseObject<string>(System.Net.HttpStatusCode.BadRequest, "Token invalid!"));

            var userRoles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtService.GenerateToken(user, userRoles.FirstOrDefault() ?? "");

            return Ok(new ResponseObject<object>
            {
                Data = new
                {
                    access_token = accessToken,
                    refresh_token = model.Token,
                },
                Message = "Get token success!",
                Status = System.Net.HttpStatusCode.OK
            });
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Unauthorized(new ResponseObject<string>(System.Net.HttpStatusCode.Unauthorized, "Invalid request!"));

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (result.Succeeded) return Ok(new ResponseObject<string>(System.Net.HttpStatusCode.OK, "Password has been changed successfully"));

            return BadRequest(new ResponseObject<string>(System.Net.HttpStatusCode.BadRequest, "Invalid request!"));
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole { Name = roleName });

            if (result.Succeeded)
            {
                return Ok(new ResponseObject<string>(System.Net.HttpStatusCode.OK, "Role create successfull!"));
            }
            return BadRequest(new ResponseObject<string>(System.Net.HttpStatusCode.BadRequest, "Invalid request!"));
        }
    }
}
