using Carts.Api.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IEnumerable<Cart>> GetAllCart()
        {
            return await _cartService.GetAllCartsAsync();
        }

        [HttpGet("by-id/{id}")]
        public async Task<Cart> GetCartById(Guid id)
        {
            return await _cartService.GetCartByIdAsync(id);
        }

        [HttpGet("by-user-id/{userId}")]
        public async Task<IEnumerable<Cart>> GetCartByUserId(Guid userId)
        {
            return await _cartService.GetCartByUserIdAsync(userId);
        }

        [HttpGet("total-price-of-cart/{userId}")]
        public async Task<double> TotalPriceOfCart(Guid userId)
        {
            return await _cartService.TotalPriceOfCartAsync(userId);
        }

        [HttpGet("count-products-of-cart/{userId}")]
        public async Task<int> CountProductsOfCart(Guid userId)
        {
            return await _cartService.CountProductsOfCartAsync(userId);
        }

        [HttpGet("shipping-fee-of-cart/{userId}")]
        public async Task<double> ShippingFeeOfCart(Guid userId)
        {
            return await _cartService.ShippingFeeOfCartAsync(userId);
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> CreateCart(Cart cart)
        {
            return await _cartService.CreateCartAsync(cart);
        }

        [HttpPut]
        public async Task UpdateCart(Cart cart)
        {
            await _cartService.UpdateCartAsync(cart);
        }

        [HttpDelete("{id}")]
        public async Task DeleteCart(Guid id)
        {
            await _cartService.DeleteCartAsync(id);
        }
    }
}
