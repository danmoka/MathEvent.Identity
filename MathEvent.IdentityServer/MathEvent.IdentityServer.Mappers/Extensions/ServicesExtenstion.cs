using Microsoft.Extensions.DependencyInjection;

namespace MathEvent.IdentityServer.Mappers.Extensions
{
    public static class ServiceExtension
    {
        /// <summary>
        /// Настройка маппера
        /// </summary>
        /// <param name="services">Зависимости</param>
        public static void ConfigureMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MathEventIdentityUserProfile));
        }
    }
}
