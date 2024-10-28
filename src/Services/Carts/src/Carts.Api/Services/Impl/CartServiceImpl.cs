using DataAccess.Base;
using DataAccess.Models;

namespace Carts.Api.Services.Impl
{
    public class CartServiceImpl : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartServiceImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CountProductsOfCartAsync(Guid userId)
        {
            return _unitOfWork.CartRepository.Get(
                    filter: u => u.UserId.Equals(userId)
                    ).Count();
        }

        public async Task<Cart> CreateCartAsync(Cart cart)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteCartAsync(Guid id)
        {
            var existingCart = _unitOfWork.CartRepository.GetByID(id);

            if (existingCart == null)
            {
                throw new KeyNotFoundException($"Cart with ID {id} not found.");
            }

            _unitOfWork.CartRepository.Delete(id);
            _unitOfWork.Save();
        }

        public async Task<IEnumerable<Cart>> GetAllCartsAsync()
        {
            return _unitOfWork.CartRepository.Get(
                    includeProperties: "Product"
                    ).ToList();
        }

        public async Task<Cart> GetCartByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Cart>> GetCartByUserIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<double> ShippingFeeOfCartAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<double> TotalPriceOfCartAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            throw new NotImplementedException();
        }

        private bool CartExists(Guid id)
        {
            return _unitOfWork.CartRepository.GetByID(id) != null ? true : false;
        }
    }
}
