using DataAccess.Models;

namespace Dashboard.Api.Services.Impl
{
    public class RevenueAnalyticsImpl : IRevenueAnalytics
    {
        public Task<decimal> GetRevenueByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetRevenueByDateRange(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetRevenueThisMonth()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetTopSellingProducts()
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetTotalRevenue()
        {
            throw new NotImplementedException();
        }
    }
}
