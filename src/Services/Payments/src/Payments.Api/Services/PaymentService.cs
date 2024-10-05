using Payments.Api.Models.DTOs;
using Payments.Api.Services.Interfaces;

namespace Payments.Api.Services
{
    public class PaymentService : IPaymentService
    {
        public Task<string> CreatePayment(CreatePaymentDTO model)
        {
            throw new NotImplementedException();
        }

        public Task MoMoResponse()
        {
            throw new NotImplementedException();
        }

        public Task VNPayResponse()
        {
            throw new NotImplementedException();
        }

        private string PayWithVNPay()
        {
            return "";
        }

        private string PayWithMoMo()
        {
            return "";
        }
    }
}
