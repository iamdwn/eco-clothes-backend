namespace EventBus.Events.Interfaces
{
    public interface IPaymentResponseEvent
    {
        bool PaymentStatus { get; set; }
        string? Transaction { get; set; }
    }
}
