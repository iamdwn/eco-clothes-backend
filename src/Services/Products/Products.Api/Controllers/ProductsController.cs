using DataAccess.Models;
using DataAccess.Models.Response;
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
        private readonly ISizeService _sizeService;
        private readonly ICategoryService _categoryService;
        public ProductsController(IProductService productService, ISizeService sizeService, ICategoryService categoryService)
        {
            _productService = productService;
            _sizeService = sizeService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productService.GetAllProductsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseObject>> GetProduct(Guid id)
        {
            try
            {
                return ResponseObject.Success<Product>(await _productService.GetProductByIdAsync(id));
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }

        [HttpGet("by-slug/{slug}")]
        public async Task<ResponseObject> GetProductBySlug(string slug)
        {
            try
            {
                return ResponseObject.Success<Product>(await _productService.GetProductBySlugAsync(slug));
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }

        [HttpGet("by-seller/{userId}")]
        public async Task<ResponseObject> GetProductBySellerId(Guid userId)
        {
            try
            {
                return ResponseObject.Success<IEnumerable<Product>>(await _productService.GetProductBySellerIdAsync(userId));
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResponseObject>> CreateProduct(RequestProduct product)
        {
            try
            {
                return ResponseObject.Success<Product>(await _productService.CreateProductAsync(product));
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }

        }

        [HttpPut]
        public async Task<ResponseObject> UpdateProduct(RequestProduct product)
        {
            try
            {
                await _productService.UpdateProductAsync(product);
                return ResponseObject.Success();
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ResponseObject> DeleteProduct(Guid id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return ResponseObject.Success();
            }
            catch (Exception ex)
            {
                return ResponseObject.Failure(ex.Message);
            }
        }

        [HttpGet("product-sizes")]
        public async Task<IEnumerable<Size>> GetSizes()
        {
            return await _sizeService.GetAllSizesAsync();
        }

        [HttpGet("product-categories")]
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _categoryService.GetAllCategoriesAsync();
        }
    }
}
