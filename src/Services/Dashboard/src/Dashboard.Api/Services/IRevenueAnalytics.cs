using DataAccess.Models;

namespace Dashboard.Api.Services
{
    public interface IRevenueAnalytics
    {
        Task<decimal> GetTotalRevenue();
        Task<decimal> GetRevenueByCategory(string category);
        Task<decimal> GetRevenueThisMonth();
        Task<decimal> GetRevenueByDateRange(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Product>> GetTopSellingProducts();
    }
}
