using System.Security.Cryptography;

namespace IdentityServer.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public RefreshToken(string token, DateTimeOffset createdDate, DateTimeOffset expiryDate, string userId)
        {
            Token = token;
            CreatedDate = createdDate;
            ExpiryDate = expiryDate;
            UserId = userId;
        }

        public static RefreshToken Create(string userId, int refreshTokenExpirationInMonths)
        {
            var token = GenerateToken();
            var createdDate = DateTimeOffset.UtcNow;
            var expiryDate = DateTimeOffset.UtcNow.AddMonths(refreshTokenExpirationInMonths);

            return new RefreshToken(token, createdDate, expiryDate, userId);
        }

        public static string GenerateToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
