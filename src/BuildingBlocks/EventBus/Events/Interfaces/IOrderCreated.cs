namespace EventBus.Events.Interfaces
{
    public interface IOrderCreated
    {
        Guid OrderId { get; }
        List<OrderItemDto> OrderItems { get; }
    }

    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
