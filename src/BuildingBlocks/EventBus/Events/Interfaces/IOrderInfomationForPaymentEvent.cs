namespace EventBus.Events.Interfaces
{
    public interface IOrderInfomationForPaymentEvent
    {
        string UserId { get; set; }
        double Amount { get; set; }
        string OrderId { get; set; }
        PaymentMethod PaymentMethod { get; set; }
    }

    public enum PaymentMethod
    {
        VNPay,
        MoMo,
        Unidentified
    }
}
