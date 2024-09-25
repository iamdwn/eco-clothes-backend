using IdentityServer.Models;
using System.Security.Claims;

namespace IdentityServer.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user, string roleName);
        string GenerateRefreshToken(ApplicationUser user);
        bool ValidateToken(string token, ApplicationUser user);
    }
}
