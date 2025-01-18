using Dashboard.Api.Dtos;
using DataAccess.Models;

namespace Dashboard.Api.Services
{
    public interface IRevenueAnalytics
    {
        Task<decimal> GetTotalRevenueAsync();
        IEnumerable<CategoryRevenueDto> GetRevenueByCategoryAsync();
        Task<decimal> GetRevenueThisMonthAsync();
        Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Product>> GetTopSellingProductsAsync();
    }
}
