using DataAccess.Models;
using Orders.Api.Dtos;

namespace Orders.Api.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetOrderByBuyerIdAsync(Guid id);
        Task<Order> CreateOrderAsync(OrderDto order);
        Task UpdateOrderAsync(OrderDto order);
        Task DeleteOrderAsync(Guid id);
    }
}
