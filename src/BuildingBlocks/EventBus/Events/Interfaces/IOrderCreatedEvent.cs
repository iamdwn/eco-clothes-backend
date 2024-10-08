using DataAccess.Models;

namespace EventBus.Events.Interfaces
{
    public interface IOrderCreatedEvent
    {
        OrderItemDto OrderItem { get; set; }
        Size SizeEntity { get; set; }
        SizeProduct ProductBySize { get; set; }
        Product ExistingProduct { get; set; }
        int Amount { get; set; }
    }

    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string? SizeName { get; set; }
    }
}
