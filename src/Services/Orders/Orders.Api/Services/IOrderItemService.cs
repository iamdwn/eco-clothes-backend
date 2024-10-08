using DataAccess.Models;
using EventBus.Events.Interfaces;

namespace Orders.Api.Services
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync();
        Task InsertOrderItem(OrderItemDto orderItemList, Guid productId);
        Task UpdateOrderItem(List<OrderItemDto>? orderItemList, Guid productId);
        Task DeleteOrderItem(Guid productId);
    }
}
