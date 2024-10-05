using System.Net;
using System.Text;

namespace Payments.Api.Services.Interfaces
{
    public interface IVnPayService
    {
        public void AddRequestData(string key, string value);
        public void AddResponseData(string key, string value);
        public string GetResponseData(string key);
        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret);
        public bool ValidateSignature(string inputHash, string secretKey);
        public string GetResponseData();
    }
}
