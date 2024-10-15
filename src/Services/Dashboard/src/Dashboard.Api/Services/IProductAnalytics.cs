using DataAccess.Models;

namespace Dashboard.Api.Services
{
    public interface IProductAnalytics
    {
        Task<IEnumerable<Product>> GetTotalProductsAsync();
        Task<IEnumerable<Product>> GetInStockProductsAsync();
        Task<int> CountInStockProductsAsync();
        Task<int> CountProductsAsync();
        Task<int> CountDailyProductsAsync(DateTime dateTime);
    }
}
