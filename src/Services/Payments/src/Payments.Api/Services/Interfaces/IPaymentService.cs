using Payments.Api.Models.DTOs;

namespace Payments.Api.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreatePayment(CreatePaymentDTO model);
        Task<string> VNPayResponse(Dictionary<string, string> queryParams);
        Task<string> MoMoResponse(Dictionary<string, string> queryParams);
        Task<string> PayOsResponse(Dictionary<string, string> queryParams);
    }
}
