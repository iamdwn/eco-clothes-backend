using DataAccess.Models;
using DataAccess.Models.Response;
using Favorites.Api.Dtos.Request;

namespace Favorites.Api.Services.Interfaces
{
    public interface IFavoriteService
    {
        Task<ResponseObject> AddProductToFavorite(AddProductToFavoriteDTO model);
        Task<ResponseObject> RemoveProductFromFavorite(RemoveProductFromFavoriteDTO model);
        Task<ResponseObject<IEnumerable<Favorite>>> GetFavoritesByUser(Guid userId);
    }
}
