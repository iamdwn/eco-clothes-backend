using IdentityServer.Models;
using System.Security.Claims;

namespace IdentityServer.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user);
        string GenerateRefreshToken(ApplicationUser user);
        bool ValidateRefreshToken(string token);
        bool ValidateToken(string token);
    }
}
