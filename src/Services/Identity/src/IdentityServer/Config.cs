using Duende.IdentityServer.Models;
using Duende.IdentityServer;
using IdentityModel;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.Email(),
                new IdentityResources.OpenId(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
                {
                    new ApiScope("api1", "My API")
                };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                    new Client
                    {
                        ClientId = "client",

                        AllowOfflineAccess = true,

                        // no interactive user, use the clientid/secret for authentication
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                        // secret for authentication
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },

                        // scopes that client has access to
                        AllowedScopes =
                        {
                            "api1",
                            IdentityServerConstants.StandardScopes.Email,
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.OfflineAccess, // refresh token
                        },
                        AccessTokenLifetime = 1 * 60 * 60,
                        RefreshTokenExpiration = TokenExpiration.Sliding,
                        RefreshTokenUsage = TokenUsage.ReUse
                    }
            };
    }
}
