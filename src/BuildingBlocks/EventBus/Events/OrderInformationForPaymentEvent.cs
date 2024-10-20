using EventBus.Events.Interfaces;

namespace EventBus.Events
{
    public class OrderInformationForPaymentEvent : IOrderInfomationForPaymentEvent
    {
        public string UserId { get; set; }
        public double Amount { get; set; }
        public string OrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
