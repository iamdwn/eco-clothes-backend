using Dashboard.Api.Dtos;
using Dashboard.Api.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardRevenueController : Controller
    {
        private readonly IRevenueAnalytics _revenueAnalytics;

        public DashboardRevenueController(IRevenueAnalytics revenueAnalytics)
        {
            _revenueAnalytics = revenueAnalytics;
        }

        [HttpGet("get-revenue-by-category")]
        public IEnumerable<CategoryRevenueDto> GetRevenueByCategory()
        {
            return _revenueAnalytics.GetRevenueByCategoryAsync();
        }

        [HttpGet("get-revenue-by-date-range")]
        public async Task<decimal> GetRevenueByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _revenueAnalytics.GetRevenueByDateRangeAsync(startDate, endDate);
        }

        [HttpGet("get-revenue-this-month")]
        public async Task<decimal> GetRevenueThisMonth()
        {
            return await _revenueAnalytics.GetRevenueThisMonthAsync();
        }

        [HttpGet("get-top-selling-products")]
        public async Task<IEnumerable<Product>> GetTopSellingProduct()
        {
            return await _revenueAnalytics.GetTopSellingProductsAsync();
        }

        [HttpGet("get-total-revenue")]
        public async Task<decimal> GetTotalRevenue()
        {
            return await _revenueAnalytics.GetTotalRevenueAsync();
        }
    }
}
