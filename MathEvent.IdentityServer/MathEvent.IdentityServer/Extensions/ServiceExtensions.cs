using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using MathEvent.IdentityServer.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace MathEvent.IdentityServer.Extensions
{
    /// <summary>
    /// Статический класс, расширяющий IServiceCollection
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Настройка IdentityServer4
        /// </summary>
        /// <param name="services">Зависимости</param>
        public static void ConfigureIdentityServer(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var password = jwtSettings["Secret"];
            var certificate = Path.Combine(env.ContentRootPath, jwtSettings["Folder"], jwtSettings["Certificate"]);

            var cert = new X509Certificate2(
              certificate,
              password,
              X509KeyStorageFlags.MachineKeySet |
              X509KeyStorageFlags.PersistKeySet |
              X509KeyStorageFlags.Exportable
            );

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.IssuerUri = jwtSettings["IssuerUri"];
            })
                .AddInMemoryIdentityResources(new List<IdentityResource>
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResources.Email(),
                })
                .AddInMemoryApiScopes(new List<ApiScope>
                {
                    new ApiScope(configuration.GetSection("MathEventApiScope")["Main"])
                })
                .AddInMemoryClients(Clients(configuration))
                .AddAspNetIdentity<MathEventIdentityUser>()
                .AddProfileService<ProfileService>()
                .AddSigningCredential(cert)
                .AddValidationKey(cert);

            services.AddSingleton<ICorsPolicyService>((container) =>
            {
                var logger = container.GetRequiredService<ILogger<DefaultCorsPolicyService>>();

                return new DefaultCorsPolicyService(logger)
                {
                    AllowedOrigins = configuration.GetSection("Origins").Get<string[]>()
                };
            });
        }

        /// <summary>
        /// Описывает клиентов, которые могут взаимодействовать с IdentityServer4
        /// </summary>
        public static IEnumerable<Client> Clients(IConfiguration configuration)
        {
            var mathEventReactClient = configuration.GetSection("ReactClient");
            var mathEvetnApiScope = configuration.GetSection("MathEventApiScope");

            return new List<Client>
            {
                new Client
                {
                    ClientId = mathEventReactClient["ClientId"],
                    ClientName = mathEventReactClient["ClientName"],
                    ClientSecrets = mathEventReactClient
                        .GetSection("ClientSecrets")
                        .Get<string[]>()
                        .Select(s => new Secret(s.Sha256()))
                        .ToList(),
                    AllowAccessTokensViaBrowser = true,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = true,
                    AllowOfflineAccess = true,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    AbsoluteRefreshTokenLifetime = 1296000,
                    AccessTokenLifetime = 3600,
                    AllowedScopes = new List<string>
                    {
                        mathEvetnApiScope["Main"],
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    }
                }
            };
        }

        /// <summary>
        /// Настройка контроллеров и json
        /// </summary>
        /// <param name="services">Зависимости</param>
        public static void ConfigureControllers(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
        }

        /// <summary>
        /// Настройка OpenApi
        /// </summary>
        /// <param name="services">Зависимости</param>
        public static void ConfigureOpenApi(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MathEvent.IdentityServer.Api", Version = "v1" });
            });
        }

        /// <summary>
        /// Настройка аутентификации
        /// </summary>
        /// <param name="services">Зависимости</param>
        /// <param name="configuration">Поставщик конфигурации</param>
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationSettings = configuration.GetSection("Authentication");
            services.AddAuthentication().AddJwtBearer(options =>
            {
                options.Authority = authenticationSettings["Authority"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = authenticationSettings["IssuerUri"],
                };
            });
        }

        /// <summary>
        /// Настройка авторизации
        /// </summary>
        /// <param name="services">Зависимости</param>
        /// <param name="configuration">Поставщик конфигурации</param>
        public static void ConfigureAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationSettings = configuration.GetSection("Authentication");

            services.AddAuthorization(options =>
            {
                options.AddPolicy("MathEventIdentityServer.Api", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", authenticationSettings
                        .GetSection("Scopes")
                        .Get<string[]>());
                });
            });
        }
    }
}
