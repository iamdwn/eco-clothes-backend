using System.Security.Cryptography;
using System.Text;

namespace Payments.Api.Utils
{
    public class GenerateHash
    {
        private readonly static string HMAC_SHA256 = "HmacSHA256";
        private readonly static string HMAC_SHA512 = "HmacSHA512";
        public static String HmacSHA(string key, string data, string algorithm)
        {
            try
            {
                if (key == null || data == null)
                {
                    throw new ArgumentNullException();
                }

                // Choose the hashing algorithm (e.g., "HMACSHA512", "HMACSHA256", etc.)
                using (var hmac = HMAC.Create(algorithm))
                {
                    if (hmac == null)
                    {
                        throw new CryptographicException($"Algorithm {algorithm} is not supported.");
                    }

                    // Convert key to bytes and initialize HMAC
                    byte[] hmacKeyBytes = Encoding.UTF8.GetBytes(key);
                    hmac.Key = hmacKeyBytes;

                    // Convert data to bytes
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                    // Compute the HMAC hash
                    byte[] result = hmac.ComputeHash(dataBytes);

                    // Convert the result to a hex string
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in result)
                    {
                        sb.Append(b.ToString("x2"));
                    }

                    return sb.ToString();
                }
            }
            catch (ArgumentNullException ex)
            {
                // Handle null argument
                Console.WriteLine("Key or data is null: " + ex.Message);
                return "";
            }
            catch (CryptographicException ex)
            {
                // Handle cryptographic exception
                Console.WriteLine("Error in HMAC calculation: " + ex.Message);
                return "";
            }
        }

        public static String HmacSHA256(string secretKey, string data)
        {
            return HmacSHA(secretKey, data, HMAC_SHA256);
        }

        public static String HmacSHA512(string secretKey, string data)
        {
            return HmacSHA(secretKey, data, HMAC_SHA512);
        }
    }
}
