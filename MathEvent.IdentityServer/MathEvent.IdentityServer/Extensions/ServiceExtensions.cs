using IdentityServer4.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace MathEvent.IdentityServer.Extensions
{
    /// <summary>
    /// Статический класс, расширяющий IServiceCollection
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Настройка подключения к базе данных
        /// </summary>
        /// <param name="services">Зависимости</param>
        /// <param name="configuration">Поставщик конфигурации</param>
        public static void ConfigureConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DBConnection"));
            });
        }

        /// <summary>
        /// Настройка IdentityServer4
        /// </summary>
        /// <param name="services">Зависимости</param>
        public static void ConfigureIdentityServer(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
        {
            var jwtSettingsPath = Path.Combine(env.ContentRootPath, configuration["Jwt"]);
            var jwtJsonString = File.ReadAllText(jwtSettingsPath);
            var jwtJObject = JObject.Parse(jwtJsonString);
            var password = jwtJObject["Jwt"]["Secret"].ToString();
            var certificate = Path.Combine(env.ContentRootPath, "Certificates", jwtJObject["Jwt"]["Certificate"].ToString());

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
                options.EmitStaticAudienceClaim = true;
            })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<IdentityUser>()
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
        /// Настройка пользователя
        /// </summary>
        /// <param name="services">Зависимости</param>
        public static void ConfigureIndentity(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<RepositoryContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
            });
        }
    }
}
