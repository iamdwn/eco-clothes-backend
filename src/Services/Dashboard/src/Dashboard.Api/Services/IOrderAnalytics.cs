using DataAccess.Models;

namespace Dashboard.Api.Services
{
    public interface IOrderAnalytics
    {
        Task<IEnumerable<Order>> GetOrdersDeliveredAsync();
        Task<IEnumerable<Order>> GetOrdersBeingDeliveredAsync();
        Task<IEnumerable<Order>> GetTotalOrdersAsync();
        Task<int> CountOrdersDeliveredAsync();
        Task<int> CountOrdersBeingDeliveredAsync();
        Task<int> CountTotalOrdersAsync();
        Task<int> CountDailyOrdersDeliveredAsync();
        Task<int> CountDailyOrdersBeingDeliveredAsync();
    }
}
