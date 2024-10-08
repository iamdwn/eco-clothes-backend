using IdentityServer.Services.Interfaces;
using System.Security.Claims;

namespace IdentityServer.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _accessor;

        public CurrentUserService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string? UserId => _accessor.HttpContext.User.Identity.Name;

        public async Task<bool> IsAuthenticated()
        {
            return await Task.FromResult(_accessor.HttpContext.User.Identity.IsAuthenticated);
        }

        public async Task<IEnumerable<Claim>> GetClaimsIdentity()
        {
            return await Task.FromResult(_accessor.HttpContext.User.Claims);
        }
    }
}
