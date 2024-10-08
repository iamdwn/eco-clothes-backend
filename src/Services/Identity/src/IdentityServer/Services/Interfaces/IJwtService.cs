using IdentityServer.Models;
using System.Security.Claims;

namespace IdentityServer.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(List<Claim> claims);
    }
}
