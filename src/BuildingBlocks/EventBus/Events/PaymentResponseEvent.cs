using EventBus.Events.Interfaces;

namespace EventBus.Events
{
    public class PaymentResponseEvent : IPaymentResponseEvent
    {
        public bool PaymentStatus { get; set; }
        public string? Transaction { get; set; }
    }
}
