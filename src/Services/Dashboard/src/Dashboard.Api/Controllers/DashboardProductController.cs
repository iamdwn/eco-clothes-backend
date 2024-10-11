using Dashboard.Api.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardProductController : Controller
    {
        private readonly IProductAnalytics _productAnalytics;

        public DashboardProductController(IProductAnalytics productAnalytics)
        {
            _productAnalytics = productAnalytics;
        }

        [HttpGet("total-products")]
        public async Task<IEnumerable<Product>> GetTotalProducts()
        {
            return await _productAnalytics.GetTotalProductsAsync();
        }

        [HttpGet("in-stock-products")]
        public async Task<IEnumerable<Product>> GetInStockProducts()
        {
            return await _productAnalytics.GetInStockProductsAsync();
        }

        [HttpGet("count-products")]
        public async Task<int> CountProducts()
        {
            return await _productAnalytics.CountProductsAsync();
        }

        [HttpGet("count-in-stock-products")]
        public async Task<int> CountInStockProducts()
        {
            return await _productAnalytics.CountInStockProductsAsync();
        }
    }
}
