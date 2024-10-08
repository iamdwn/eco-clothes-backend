using DataAccess.Models;
using EventBus.Events.Interfaces;

namespace EventBus.Events
{
    public class OrderCreatedEvent : IOrderCreatedEvent
    {
        public OrderItemDto? OrderItem { get; set; }
        public Size? SizeEntity { get; set; }
        public SizeProduct? ProductBySize { get; set; }
        public Product? ExistingProduct { get; set; }
        public int Amount { get; set; }
    }
}
