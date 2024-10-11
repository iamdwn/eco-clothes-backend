using Microsoft.AspNetCore.Mvc;
using Users.Api.Dtos.Request;
using Users.Api.Services.Interfaces;

namespace Users.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUsetDTO model)
        {
            return Ok();
        }

        [HttpPost("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            return Ok();
        }

        [HttpPost("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            return Ok();
        }

        [HttpPost("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok();
        }
    }
}
