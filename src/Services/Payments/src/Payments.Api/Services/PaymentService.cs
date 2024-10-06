using DataAccess.Base;
using DataAccess.Models;
using Microsoft.AspNetCore.Components.Forms;
using Payments.Api.Models.DTOs;
using Payments.Api.Services.Interfaces;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Payments.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, ILogger<PaymentService> logger, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public string CreatePayment(CreatePaymentDTO model)
        {
            switch (model.PaymentMethod)
            {
                case PaymentMethod.VNPay:
                    return PayWithVNPay(model.Amount, model.Id);
                case PaymentMethod.MoMo:
                    return PayWithMoMo();
                default:
                    throw new NotImplementedException();
            }
        }

        public void MoMoResponse()
        {
            throw new NotImplementedException();
        }

        public void VNPayResponse(Dictionary<string, string> queryParams)
        {
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
                switch (vnp_Responses["vnp_ResponseCode"])
                {
                    case "00":
                        _logger.LogInformation("Giao dich thanh cong");
                        var payment = new Payment
                        {
                            Amount = decimal.Parse(vnp_Responses["vnp_Amount"]),
                            Method = "VNPay",
                            Status = vnp_Responses["vnp_TransactionStatus"],
                            TransactionId = vnp_Responses["vnp_TransactionNo"]
                        };
                        _unitOfWork.PaymentRepository.Insert(payment);
                        _unitOfWork.Save();
                        break;

                    case "01":
                        _logger.LogInformation("Giao dich chua hoan tat");
                        break;

                    case "02":
                        _logger.LogInformation("Giao dich bi loi");
                        break;

                    default:
                        _logger.LogInformation("Co loi xay ra");
                        break;
                }
            }
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
            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, signData);
            baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

            return baseUrl;
        }

        private string PayWithMoMo()
        {
            return "";
        }

        private bool CheckVNPayPayment(string inputHash, string rspRaw)
        {
            string myChecksum = HmacSHA512(_configuration["Payment:VNPay:HashSecret"], rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private void CheckMoMoPayment()
        {
            throw new NotImplementedException();
        }

        private string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
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
