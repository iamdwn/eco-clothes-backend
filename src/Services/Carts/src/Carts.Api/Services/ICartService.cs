using Carts.Api.Dtos;
using DataAccess.Models;

namespace Carts.Api.Services
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetAllCartsAsync();
        Task<Cart> GetCartByIdAsync(Guid id);
        Task<IEnumerable<Cart>> GetCartByUserIdAsync(Guid id);
        Task<double> TotalPriceOfCartAsync(Guid userId);
        Task<int> CountProductsOfCartAsync(Guid userId);
        Task<double> ShippingFeeOfCartAsync(Guid userId);
        Task<Cart> AddToCartAsync(CartDto cart);
        //Task UpdateCartAsync(Cart cart);
        Task DeleteCartAsync(Guid userId);
    }
}
