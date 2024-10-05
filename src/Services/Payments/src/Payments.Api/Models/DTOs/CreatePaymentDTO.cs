namespace Payments.Api.Models.DTOs
{
    public class CreatePaymentDTO
    {
        public string UserId { get; set; }
        public double Amount { get; set; }
    }
}
