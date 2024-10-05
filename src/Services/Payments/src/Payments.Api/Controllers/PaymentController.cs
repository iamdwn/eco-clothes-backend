using EventBus.Response;
using Microsoft.AspNetCore.Mvc;
using Payments.Api.Services;

using Payments.Api.Services.Interfaces;

namespace Payments.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IConfiguration _configuration;

        public PaymentController(IVnPayService vpnPayService, IConfiguration configuration)
        {
            _vnPayService = vpnPayService;
            _configuration = configuration;
        }

        [HttpPost("CreatePayment")]
        public IActionResult CreatePayment(decimal amount, string orderId)
        {
            _vnPayService.AddRequestData("vnp_Version", "2.1.0");
            _vnPayService.AddRequestData("vnp_Command", "pay");
            _vnPayService.AddRequestData("vnp_TmnCode", _configuration["Payment:VNPay:TmnCode"]);
            _vnPayService.AddRequestData("vnp_Amount", (amount * 100).ToString()); // Số tiền cần nhân với 100
            _vnPayService.AddRequestData("vnp_CurrCode", "VND");
            _vnPayService.AddRequestData("vnp_TxnRef", orderId);
            _vnPayService.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang {orderId}");
            _vnPayService.AddRequestData("vnp_OrderType", "other");
            _vnPayService.AddRequestData("vnp_Locale", "vn");
            _vnPayService.AddRequestData("vnp_ReturnUrl", "https://localhost:7135/api/Payment/VnPayResponse"); // URL callback khi thanh toán xong
            _vnPayService.AddRequestData("vnp_IpAddr", Request.HttpContext.Connection.RemoteIpAddress?.ToString());
            _vnPayService.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));

            string paymentUrl = _vnPayService.CreateRequestUrl(_configuration["Payment:VNPay:Url"], _configuration["Payment:VNPay:HashSecret"]);
            return Ok(ResponseObject.Success<string>(paymentUrl));
        }

        [HttpGet("VnPayResponse")]
        public IActionResult VnPayResponse([FromQuery] Dictionary<string, string> queryParams)
        {
            string vnp_SecureHash = queryParams["vnp_SecureHash"];
            string vnp_HashSecret = _configuration["Payment:VNPay:HashSecret"];

            foreach (var s in queryParams.Keys)
            {
                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                {
                    _vnPayService.AddResponseData(s, queryParams[s]);
                }
            }

            bool isValidSignature = _vnPayService.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

            if (isValidSignature)
            {
                return Ok("Payment Success");
            }
            else
            {
                return BadRequest("Invalid signature");
            }
        }
    }
}
