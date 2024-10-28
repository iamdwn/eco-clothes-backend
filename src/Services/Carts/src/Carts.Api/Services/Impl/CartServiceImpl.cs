using Carts.Api.Dtos;
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

        public async Task<Cart> AddToCartAsync(CartDto cart)
        {
            var size = _unitOfWork.SizeRepository.Get(
                        filter: s => s.Name.Equals(cart.SizeName)
                        ).FirstOrDefault();

            if (size == null) throw new Exception($"Not found size with name {cart.SizeName}");

            var existingCart = _unitOfWork.CartRepository.Get(
                         filter: p => p.UserId.Equals(cart.UserId)
                         ).FirstOrDefault();

            if (existingCart == null)
            {
                var insertCart = new Cart()
                {
                    UserId = cart.UserId,
                    ProductId = cart.ProductId,
                    SizeId = size.SizeId,
                    Quantity = cart.Quantity,
                    Price = cart.Price
                };

                _unitOfWork.CartRepository.Insert(insertCart);
                _unitOfWork.Save();

                return insertCart;
            }

            existingCart.UserId = cart.UserId ?? existingCart.UserId;
            existingCart.ProductId = cart.ProductId ?? existingCart.ProductId;
            existingCart.SizeId = size.SizeId;
            existingCart.Quantity = cart.Quantity ?? existingCart.Quantity;
            existingCart.Price = cart.Price ?? existingCart.Price;

            _unitOfWork.CartRepository.Update(existingCart);
            _unitOfWork.Save();
            return existingCart;
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
            var existingCart = _unitOfWork.CartRepository.Get(
                filter: p => p.CartId.Equals(id),
                includeProperties: "Product"
                ).FirstOrDefault();

            if (existingCart == null)
            {
                throw new KeyNotFoundException($"Cart with ID {id} not found.");
            }
            return existingCart;
        }

        public async Task<IEnumerable<Cart>> GetCartByUserIdAsync(Guid userId)
        {
            return _unitOfWork.CartRepository.Get(
                    filter: u => u.UserId.Equals(userId)
                    ).ToList();
        }

        public async Task<double> ShippingFeeOfCartAsync(Guid userId)
        {
            double shippingRate = 0.04;
            double totalPrice = TotalPriceOfCartAsync(userId).Result;
            double shippingFee = totalPrice * shippingRate;
            return totalPrice + shippingFee;
        }

        public async Task<double> TotalPriceOfCartAsync(Guid userId)
        {
            return _unitOfWork.CartRepository.Get(
                    filter: u => u.UserId.Equals(userId),
                    includeProperties: "Product"
                    ).Sum(c => c.Price ?? 0); ;
        }

        private bool CartExists(Guid id)
        {
            return _unitOfWork.CartRepository.GetByID(id) != null ? true : false;
        }
    }
}
