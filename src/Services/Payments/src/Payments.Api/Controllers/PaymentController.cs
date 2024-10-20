using DataAccess.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Payments.Api.Models.DTOs;
using Payments.Api.Services.Interfaces;

namespace Payments.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("CreatePayment")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDTO model)
        {
            return Ok(ResponseObject.Success<string>(await _paymentService.CreatePayment(model)));
        }

        [HttpGet("VnPayResponse")]
        public async Task<IActionResult> VnPayResponse([FromQuery] Dictionary<string, string> queryParams)
        {
            string url = await _paymentService.VNPayResponse(queryParams);
            return Redirect(url);
        }

        [HttpGet("MoMoResponse")]
        public async Task<IActionResult> MoMoResponse([FromQuery] Dictionary<string, string> queryParams)
        {
            string url = await _paymentService.MoMoResponse(queryParams);
            return Redirect(url);
        }
    }
}
