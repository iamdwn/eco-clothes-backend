using Favorites.Api.Dtos.Request;
using Favorites.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Favorites.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFavoritesByUser([FromRoute] Guid userId)
        {
            var user = User.Identity?.Name;
            var role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            return Ok(await _favoriteService.GetFavoritesByUser(userId));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProductToFavorite([FromBody] AddProductToFavoriteDTO model)
        {
            return Ok(await _favoriteService.AddProductToFavorite(model));
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> RemoveProductFromFavorite([FromBody] RemoveProductFromFavoriteDTO model)
        {
            return Ok(await _favoriteService.RemoveProductFromFavorite(model));
        }
    }
}
