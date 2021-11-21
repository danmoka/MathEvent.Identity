using MathEvent.IdentityServer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace MathEvent.IdentityServer.Database.Extensions
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
        /// Настройка пользователя
        /// </summary>
        /// <param name="services">Зависимости</param>
        public static void ConfigureIndentity(this IServiceCollection services)
        {
            services.AddIdentity<MathEventIdentityUser, MathEventIdentityRole>()
                .AddEntityFrameworkStores<RepositoryContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.Email;
            });
        }
    }
}
