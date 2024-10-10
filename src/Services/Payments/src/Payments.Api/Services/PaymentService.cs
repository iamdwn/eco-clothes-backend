using DataAccess.Base;
using DataAccess.Models;
using EventBus.Events;
using Newtonsoft.Json;
using Payments.Api.Models.DTOs;
using Payments.Api.Services.Interfaces;
using Payments.Api.Utils;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Payments.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentService> _logger;
        private readonly IMassTransitService _massTransitService;

        public PaymentService(IConfiguration configuration, ILogger<PaymentService> logger, IUnitOfWork unitOfWork, IMassTransitService massTransitService)
        {
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _massTransitService = massTransitService;
        }

        public async Task<string> CreatePayment(CreatePaymentDTO model)
        {
            switch (model.PaymentMethod)
            {
                case PaymentMethod.VNPay:
                    return PayWithVNPay(model.Amount, model.Id);
                case PaymentMethod.MoMo:
                    return await PayWithMoMo(model.Amount, model.Id);
                default:
                    throw new NotImplementedException();
            }
        }

        public Task<string> MoMoResponse(Dictionary<string, string> queryParams)
        {
            string baseUrl = "http://localhost:3000";
            return Task.FromResult("");
        }

        public async Task<string> VNPayResponse(Dictionary<string, string> queryParams)
        {
            string baseUrl = "http://localhost:3000";

            SortedList<string, string> vnp_Responses = new SortedList<string, string>(new VnPayCompare());
            foreach (var s in queryParams.Keys)
            {
                if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                {
                    vnp_Responses.Add(s, queryParams[s]);
                }
            }

            StringBuilder data = new StringBuilder();
            if (vnp_Responses.ContainsKey("vnp_SecureHashType"))
            {
                vnp_Responses.Remove("vnp_SecureHashType");
            }
            if (vnp_Responses.ContainsKey("vnp_SecureHash"))
            {
                vnp_Responses.Remove("vnp_SecureHash");
            }
            foreach (KeyValuePair<string, string> kv in vnp_Responses)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }

            var isValidSignature = CheckVNPayPayment(queryParams["vnp_SecureHash"], data.ToString());
            if (isValidSignature)
            {
                if (vnp_Responses["vnp_ResponseCode"] == "00" && vnp_Responses["vnp_TransactionStatus"] == "00")
                {
                    var payment = new Payment
                    {
                        Amount = decimal.Parse(vnp_Responses["vnp_Amount"]),
                        Method = "VNPay",
                        Status = vnp_Responses["vnp_TransactionStatus"],
                        TransactionId = vnp_Responses["vnp_TransactionNo"]
                    };
                    _unitOfWork.PaymentRepository.Insert(payment);
                    _unitOfWork.Save();
                    await _massTransitService.Publish(new PaymentResponseEvent
                    {
                        PaymentStatus = true,
                        Transaction = vnp_Responses["vnp_TransactionNo"]
                    });

                    return baseUrl + $"?amount={vnp_Responses["vnp_Amount"]}&createDate={vnp_Responses["vnp_PayDate"]}&status={true}";
                }
                return baseUrl + $"?status={false}&errorMessage={"Something went wrong in process payment"}&errorCode={vnp_Responses["vnp_ResponseCode"]}";
            }
            return baseUrl + $"?status={false}&errorMessage={"Invalid signature"}";
        }

        private string PayWithVNPay(double amount, string id)
        {
            SortedList<string, string> vnp_Params = new SortedList<string, string>(new VnPayCompare())
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
                { "vnp_ReturnUrl", _configuration["Payment:VNPay:ReturnUrl"] }, // URL callback khi thanh toán xong
                { "vnp_IpAddr", "abc" },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss") }
            };

            string baseUrl = _configuration["Payment:VNPay:Url"];
            string vnp_HashSecret = _configuration["Payment:VNPay:HashSecret"];

            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in vnp_Params)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            string queryString = data.ToString();

            baseUrl += "?" + queryString;
            string signData = queryString;
            if (signData.Length > 0)
            {

                signData = signData.Remove(data.Length - 1, 1);
            }
            string vnp_SecureHash = GenerateHash.HmacSHA512(vnp_HashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

            return baseUrl;
        }

        private async Task<string> PayWithMoMo(double amount, string id)
        {
            string orderInfo = "Eco-Clothes - Pay with MoMo";
            string requestId = DateTime.UtcNow.Ticks.ToString();
            var rawData =
                $"partnerCode={_configuration["Payment:MoMo:PartnerCode"]}" +
                $"&accessKey={_configuration["Payment:MoMo:AccessKey"]}" +
                $"&requestId={requestId}" +
                $"&amount={amount}" +
                $"&orderId={id}" +
                $"&orderInfo={orderInfo}" +
                $"&returnUrl={_configuration["Payment:MoMo:ReturnUrl"]}" +
                $"&notifyUrl={_configuration["Payment:MoMo:NotifyUrl"]}" +
                $"&extraData=";

            var signature = GenerateHash.HmacSHA256(_configuration["Payment:MoMo:SecretKey"], rawData);

            var requestData = new
            {
                accessKey = _configuration["Payment:MoMo:AccessKey"],
                partnerCode = _configuration["Payment:MoMo:PartnerCode"],
                requestType = _configuration["Payment:MoMo:RequestType"],
                notifyUrl = _configuration["Payment:MoMo:NotifyUrl"],
                returnUrl = _configuration["Payment:MoMo:ReturnUrl"],
                orderId = id,
                amount = amount.ToString(),
                orderInfo,
                requestId,
                extraData = "",
                signature
            };

            using (var client = new HttpClient())
            {
                var jsonRequest = JsonConvert.SerializeObject(requestData);
                var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                var momoResponse = await client.PostAsync(_configuration["Payment:MoMo:Url"], httpContent);
                var responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(await momoResponse.Content.ReadAsStringAsync());

                return responseData["payUrl"] ?? "";
            }
        }

        private bool CheckVNPayPayment(string inputHash, string rspRaw)
        {
            string myChecksum = GenerateHash.HmacSHA512(_configuration["Payment:VNPay:HashSecret"], rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private void CheckMoMoPayment()
        {
            throw new NotImplementedException();
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
