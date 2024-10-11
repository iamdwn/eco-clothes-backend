namespace Payments.Api.Models.DTOs
{
    public class CreatePaymentDTO
    {
        public string UserId { get; set; }
        public double Amount { get; set; }
        public string Id { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }

    public enum PaymentMethod
    {
        VNPay,
        MoMo,
        Unidentified
    }
}
