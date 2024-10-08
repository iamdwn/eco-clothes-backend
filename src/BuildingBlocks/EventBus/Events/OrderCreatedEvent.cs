using DataAccess.Models;
using EventBus.Events.Interfaces;

namespace EventBus.Events
{
    public class OrderCreatedEvent : IOrderCreatedEvent
    {
        public List<OrderItemDto>? OrderItems { get; set; }
        public Size SizeEntity { get; set; }
        public SizeProduct ProductBySize { get; set; }
        public Product ExistingProduct { get; set; }
    }
}
