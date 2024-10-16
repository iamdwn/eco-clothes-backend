using Dashboard.Api.Dtos;
using DataAccess.Models;

namespace Dashboard.Api.Services
{
    public interface IRevenueAnalytics
    {
        Task<decimal> GetTotalRevenue();
        IEnumerable<CategoryRevenueDto> GetRevenueByCategory();
        Task<decimal> GetRevenueThisMonth();
        Task<decimal> GetRevenueByDateRange(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Product>> GetTopSellingProducts();
    }
}
