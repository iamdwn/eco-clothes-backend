using EventBus.Events;
using IdentityServer.Models;
using IdentityServer.Models.DTOs;
using IdentityServer.Services;
using IdentityServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IMassTransitService _massTransitService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMassTransitService massTransitService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _massTransitService = massTransitService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Ok("User logged in!");
            }
            else
            {
                return BadRequest("Invalid login attempt.");
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                ImgUrl = model.ImgUrl,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                Task t1 = _massTransitService.Publish(new UserCreatedEvent
                {
                    UserId = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    CallbackUrl = callbackUrl,
                });

                Task t2 = _signInManager.SignInAsync(user, isPersistent: false);

                await Task.WhenAll(t1, t2);

                return Ok("User create account successfully!");
            }

            return BadRequest("Invalid credentials!");
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("User logged out.");
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest("Invalid request!");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest("User doesn't exist!");
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded ? Ok("ConfirmEmail") : BadRequest("Error");
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                return BadRequest("Invalid Email Address!");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            //var callbackUrl = Url.Action("ResetPassword", "Auth", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

            await _massTransitService.Publish(new UserPasswordResetOccurredEvent
            {
                Email = user.Email,
                Code = code
            });

            return Ok("Code: " + code);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("User doesn't exist!");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok("Successfully!");
            }

            return BadRequest("Something went wrong!");
        }

        //[HttpGet("CurrentUser")]
        //public async Task<IActionResult> CurrentUser()
        //{
        //    var user = await _userManager.GetUserAsync(HttpContext.User);
        //    return Ok(user);
        //}
    }
}
