using DataAccess.Models;

namespace Dashboard.Api.Services
{
    public interface IOrderAnalytics
    {
        Task<IEnumerable<Order>> GetOrdersDeliveredAsync();
        Task<IEnumerable<Order>> GetOrdersBeingDeliveredAsync();
        Task<int> CountOrdersDeliveredAsync();
        Task<int> CountOrdersBeingDeliveredAsync();
    }
}
