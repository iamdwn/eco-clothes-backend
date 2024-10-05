using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Dtos.Request;
using Products.Api.Services;

namespace Products.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productService.GetAllProductsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            return await _productService.GetProductByIdAsync(id);
        }

        [HttpGet("by-seller/{userId}")]
        public async Task<IEnumerable<Product>> GetProductBySellerId(Guid userId)
        {
            return await _productService.GetProductBySellerIdAsync(userId);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(RequestProduct product)
        {
            return await _productService.CreateProductAsync(product);
        }

        [HttpPut]
        public async Task UpdateProduct(RequestProduct product)
        {
            await _productService.UpdateProductAsync(product);
        }

        [HttpDelete("{id}")]
        public async Task DeleteProduct(Guid id)
        {
            await _productService.DeleteProductAsync(id);
        }
    }
}
