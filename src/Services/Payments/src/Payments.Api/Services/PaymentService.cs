using DataAccess.Models;
using Payments.Api.Models.DTOs;
using Payments.Api.Services.Interfaces;
using System.Net;
using System.Text;

namespace Payments.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;

        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string> CreatePayment(CreatePaymentDTO model)
        {
            switch (model.PaymentMethod)
            {
                case PaymentMethod.VNPay:
                    return Task.FromResult(PayWithVNPay(model.Amount, model.Id));
                case PaymentMethod.MoMo:
                    return Task.FromResult(PayWithMoMo());
                default:
                    throw new NotImplementedException();
            }
        }

        public Task MoMoResponse()
        {
            throw new NotImplementedException();
        }

        public Task VNPayResponse()
        {
            throw new NotImplementedException();
        }

        private string PayWithVNPay(double amount, string id)
        {
            Dictionary<string, string> vnp_Params = new Dictionary<string, string>
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", _configuration["Payment:VNPay:TmnCode"] },
                { "vnp_Amount", (amount * 100).ToString() }, // Số tiền cần nhân với 100
                { "vnp_CurrCode", "VND" },
                { "vnp_TxnRef", id },
                { "vnp_OrderInfo", $"Thanh toan don hang {id}" },
                { "vnp_OrderType", "other" },
                { "vnp_Locale", "vn" },
                { "vnp_ReturnUrl", "https://localhost:7135/api/Payment/VnPayResponse" }, // URL callback khi thanh toán xong
                { "vnp_IpAddr", "abc" },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss") }
            };

            string baseUrl = _configuration["Payment:VNPay:Url"];
            string vnp_HashSecret = _configuration["Payment:VNPay:HashSecret"];

            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in vnp_Params)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            string queryString = data.ToString();

            baseUrl += "?" + queryString;
            String signData = queryString;
            if (signData.Length > 0)
            {

                signData = signData.Remove(data.Length - 1, 1);
            }
            string vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

            return baseUrl;
        }

        private string PayWithMoMo()
        {
            return "";
        }
    }
}
