using DataAccess.Base;
using DataAccess.Models;
using DataAccess.Models.Response;
using Favorites.Api.Dtos.Request;
using Favorites.Api.Services.Interfaces;

namespace Favorites.Api.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FavoriteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ResponseObject> AddProductToFavorite(AddProductToFavoriteDTO model)
        {
            var existProduct = _unitOfWork.ProductRepository.GetByID(model.ProductId);
            if (existProduct == null) return Task.FromResult(ResponseObject.Failure("Product does not exist!"));

            var isFavorited = _unitOfWork.FavoriteRepository.Get(filter: f => f.UserId == model.UserId && f.ProductId == model.ProductId).Any();
            if (isFavorited) return Task.FromResult(ResponseObject.Failure("Product was favorited!"));

            var newFavorite = new Favorite
            {
                UserId = model.UserId,
                ProductId = model.ProductId
            };
            _unitOfWork.FavoriteRepository.Insert(newFavorite);
            _unitOfWork.Save();

            return Task.FromResult(ResponseObject.Success("Add favorited success!"));
        }

        public Task<ResponseObject<IEnumerable<Favorite>>> GetFavoritesByUser(Guid userId)
        {
            var favorites = _unitOfWork.FavoriteRepository.Get(filter: f => f.UserId == userId);
            return Task.FromResult(ResponseObject.Success(favorites));
        }

        public Task<ResponseObject> RemoveProductFromFavorite(RemoveProductFromFavoriteDTO model)
        {
            var existProduct = _unitOfWork.ProductRepository.GetByID(model.ProductId);
            if (existProduct == null) return Task.FromResult(ResponseObject.Failure("Product does not exist!"));

            var favorite = _unitOfWork.FavoriteRepository.Get(filter: f => f.UserId == model.UserId && f.ProductId == model.ProductId).FirstOrDefault();
            if (favorite == null) return Task.FromResult(ResponseObject.Failure("Product does not favorited!"));

            _unitOfWork.FavoriteRepository.Delete(favorite);
            _unitOfWork.Save();

            return Task.FromResult(ResponseObject.Success("Remove favorited success!"));
        }
    }
}
