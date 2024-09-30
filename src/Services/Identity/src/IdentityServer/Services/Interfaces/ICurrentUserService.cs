using System.Security.Claims;

namespace IdentityServer.Services.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }

        Task<bool> IsAuthenticated();

        Task<IEnumerable<Claim>> GetClaimsIdentity();
    }
}
