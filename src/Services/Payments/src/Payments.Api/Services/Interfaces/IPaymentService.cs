using Payments.Api.Models.DTOs;

namespace Payments.Api.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreatePayment(CreatePaymentDTO model);
        Task VNPayResponse();
        Task MoMoResponse();
    }
}
