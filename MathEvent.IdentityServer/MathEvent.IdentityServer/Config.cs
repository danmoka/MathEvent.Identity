using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace MathEvent.IdentityServer
{
    /// <summary>
    /// Статический класс конфигурации для IdentityServer4
    /// </summary>
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("matheventapi", "Math Event API")
            };

        /// <summary>
        /// Описывает клиентов, которые могут взаимодействовать с IdentityServer4
        /// </summary>
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    // TODO: сделать начитку из json
                    // TODO: настройка токенов (время жизни токена доступа (меньше), а рефреш токена?)
                    ClientId = "react_spa",
                    ClientName = "React SPA",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowAccessTokensViaBrowser = true,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = false,
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    AbsoluteRefreshTokenLifetime = 1296000,
                    AccessTokenLifetime = 300,
                    AllowedScopes = new List<string>
                    {
                        "matheventapi",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    }
                }
            };
    }
}
