using Carts.Api.Dtos;
using Carts.Api.Services;
using DataAccess.Models;
using DataAccess.Models.Response;
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
        public async Task<ResponseObject> GetCartById(Guid id)
        {
            try
            {
                return ResponseObject.Success<Cart>(await _cartService.GetCartByIdAsync(id));
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }

        [HttpGet("by-user-id/{userId}")]
        public async Task<ResponseObject> GetCartByUserId(Guid userId)
        {
            try
            {
                return ResponseObject.Success<IEnumerable<Cart>>(await _cartService.GetCartByUserIdAsync(userId));
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }

        [HttpGet("total-price-of-cart/{userId}")]
        public async Task<ResponseObject> TotalPriceOfCart(Guid userId)
        {
            try
            {
                return ResponseObject.Success<double>(await _cartService.TotalPriceOfCartAsync(userId));
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }

        [HttpGet("count-products-of-cart/{userId}")]
        public async Task<ResponseObject> CountProductsOfCart(Guid userId)
        {
            try
            {
                return ResponseObject.Success<int>(await _cartService.CountProductsOfCartAsync(userId));
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }

        [HttpGet("shipping-fee-of-cart/{userId}")]
        public async Task<ResponseObject> ShippingFeeOfCart(Guid userId)
        {
            try
            {
                return ResponseObject.Success<double>(await _cartService.ShippingFeeOfCartAsync(userId));
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ResponseObject> AddToCart(CartDto cart)
        {
            try
            {
                return ResponseObject.Success<Cart>(await _cartService.AddToCartAsync(cart));
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async Task<ResponseObject> DeleteCart(Guid userId)
        {
            try
            {
                await _cartService.DeleteCartAsync(userId);
                return ResponseObject.Success();
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }
    }
}
