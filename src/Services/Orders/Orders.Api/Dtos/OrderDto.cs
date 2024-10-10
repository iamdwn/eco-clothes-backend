using EventBus.Events.Interfaces;

namespace Orders.Api.Dtos
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Address { get; set; }
        public List<OrderItemDto>? OrderItems { get; set; }
    }
}
