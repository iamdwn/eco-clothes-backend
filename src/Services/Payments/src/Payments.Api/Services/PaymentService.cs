using DataAccess.Base;
using DataAccess.Enums;
using DataAccess.Models;
using EventBus.Events;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json;
using Payments.Api.Models.DTOs;
using Payments.Api.Services.Interfaces;
using Payments.Api.Utils;
using System.Globalization;
using System.Net;
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
                    return PayWithVNPay(model.Amount, model.OrderId);
                case PaymentMethod.MoMo:
                    return await PayWithMoMo(model.Amount, model.OrderId);
                case PaymentMethod.PayOs:
                    return await PayWithPayOs(model.OrderId, model.Amount, model.Description);
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
            string baseUrl = "http://localhost:3000/order-success";

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
                        Transaction = vnp_Responses["vnp_TransactionNo"],
                    });

                    return baseUrl + $"?amount={vnp_Responses["vnp_Amount"]}&createDate={vnp_Responses["vnp_PayDate"]}&status={true}";
                }
                return baseUrl + $"?status={false}&errorMessage={Uri.EscapeDataString("Something went wrong in process payment")}&errorCode={vnp_Responses["vnp_ResponseCode"]}";
            }
            return baseUrl + $"/order-fail?status={false}&errorMessage={Uri.EscapeDataString("Invalid signature")}";
        }

        public async Task<string> PayOsResponse(Dictionary<string, string> queryParams)
        {
            string baseUrl = "https://eco-clothes.hdang09.me";

            var clientId = _configuration["Payment:PayOs:ClientID"];
            var apiKey = _configuration["Payment:PayOs:APIKey"];
            var checksumKey = _configuration["Payment:PayOs:ChecksumKey"];

            if (queryParams["code"] != null && int.TryParse(queryParams["code"], out int code))
            {
                if (code != 01)
                {
                    return baseUrl + $"/order-fail?status={false}&errorMessage={Uri.EscapeDataString("Something went wrong in process payment")}";
                }

                var orderCode = long.Parse(queryParams["orderCode"]);

                PayOS payOS = new PayOS(clientId, apiKey, checksumKey);
                PaymentLinkInformation paymentLinkInformation = await payOS.getPaymentLinkInformation(orderCode);

                var payment = new Payment
                {
                    Amount = paymentLinkInformation.amountPaid,
                    Method = "PayOs",
                    Status = paymentLinkInformation.status,
                };
                _unitOfWork.PaymentRepository.Insert(payment);
                _unitOfWork.Save();

                await _massTransitService.Publish(new PaymentResponseEvent
                {
                    PaymentStatus = paymentLinkInformation.status.Equals("PAID"),
                    OrderCode = paymentLinkInformation.orderCode,
                    Transaction = Guid.NewGuid().ToString(),
                });

                switch (paymentLinkInformation.status)
                {
                    case "PAID":
                        return baseUrl + $"/order-success?amount={paymentLinkInformation.amountPaid}&createDate={paymentLinkInformation.createdAt}&status={true}";
                    case "CANCELLED":
                        return baseUrl + $"/order-fail?status={false}&errorMessage{Uri.EscapeDataString(paymentLinkInformation.cancellationReason ?? "Payment cancel")}";
                    default:
                        return baseUrl + $"/order-fail?status={false}&errorMessage={Uri.EscapeDataString("Invalid signature")}";
                }
            }
            return baseUrl + $"/order-fail?status={false}&errorMessage={Uri.EscapeDataString("Invalid signature")}";
        }

        private string PayWithVNPay(double amount, string id)
        {
            // Define the time zone you want to use, for example, SE Asia Standard Time for Vietnam
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Convert DateTime.Now to the specified time zone
            DateTime createDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZone);
            DateTime expireDate = createDate.AddMinutes(15);

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
                { "vnp_CreateDate", createDate.ToString("yyyyMMddHHmmss") },
                { "vnp_ExpireDate", expireDate.ToString("yyyyMMddHHmmss") }
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
            var accessKey = _configuration["Payment:MoMo:AccessKey"];
            var partnerCode = _configuration["Payment:MoMo:PartnerCode"];
            var requestType = _configuration["Payment:MoMo:RequestType"];
            var notifyUrl = _configuration["Payment:MoMo:NotifyUrl"];
            var returnUrl = _configuration["Payment:MoMo:ReturnUrl"];
            var extraData = "";
            var rawData =
                $"partnerCode={partnerCode}" +
                $"&accessKey={accessKey}" +
                $"&requestId={requestId}" +
                $"&amount={amount}" +
                $"&orderId={id}" +
                $"&orderInfo={orderInfo}" +
                $"&returnUrl={returnUrl}" +
                $"&notifyUrl={notifyUrl}" +
                $"&extraData={extraData}";

            var signature = GenerateHash.HmacSHA256(_configuration["Payment:MoMo:SecretKey"], rawData);

            var requestData = new
            {
                accessKey,
                partnerCode,
                requestType,
                notifyUrl,
                returnUrl,
                orderId = id,
                amount = amount.ToString(),
                orderInfo,
                requestId,
                extraData,
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

        private async Task<string> PayWithPayOs(string orderId, double amount, string description)
        {
            var clientId = _configuration["Payment:PayOs:ClientID"];
            var apiKey = _configuration["Payment:PayOs:APIKey"];
            var checksumKey = _configuration["Payment:PayOs:ChecksumKey"];
            PayOS payOS = new PayOS(clientId, apiKey, checksumKey);

            Guid guid = Guid.Parse(orderId);
            byte[] guidBytes = guid.ToByteArray();
            long orderCode = BitConverter.ToInt32(guidBytes, 0);

            PaymentData paymentData = new PaymentData(
                orderCode,
                (int)amount,
                description,
                new List<ItemData>(),
                _configuration["Payment:PayOs:CancelUrl"] ?? "",
                _configuration["Payment:PayOs:ReturnUrl"] ?? ""
            );
            CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);
            return createPayment.checkoutUrl;
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
