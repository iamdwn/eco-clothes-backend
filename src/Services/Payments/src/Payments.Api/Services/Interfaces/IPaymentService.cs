using Payments.Api.Models.DTOs;

namespace Payments.Api.Services.Interfaces
{
    public interface IPaymentService
    {
        string CreatePayment(CreatePaymentDTO model);
        string VNPayResponse(Dictionary<string, string> queryParams);
        void MoMoResponse();
    }
}
