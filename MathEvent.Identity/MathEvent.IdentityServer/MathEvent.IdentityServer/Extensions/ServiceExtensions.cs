using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
        public static void ConfigureIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
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
                .AddDeveloperSigningCredential(); // not recommended for production - you need to store your key material somewhere secure

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
