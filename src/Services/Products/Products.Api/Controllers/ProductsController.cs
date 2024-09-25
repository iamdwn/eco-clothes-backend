using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            return await _productService.GetProductByIdAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            return await _productService.CreateProductAsync(product);
        }

        [HttpPut]
        public async Task UpdateProduct(Product product)
        {
            await _productService.UpdateProductAsync(product);
        }

        [HttpDelete("{id}")]
        public async Task DeleteProduct(string id)
        {
            await _productService.DeleteProductAsync(id);
        }
    }
}
