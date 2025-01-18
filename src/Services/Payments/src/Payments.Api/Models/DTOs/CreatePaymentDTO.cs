using DataAccess.Enums;

namespace Payments.Api.Models.DTOs
{
    public class CreatePaymentDTO
    {
        public string UserId { get; set; }
        public double Amount { get; set; }
        public string OrderId { get; set; }
        public string Description { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
