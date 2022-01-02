using MathEvent.IdentityServer.Contracts.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MathEvent.IdentityServer.Repositories.Extensions
{
    public static class ServiceExtension
    {
        /// <summary>
        /// Настройка репозитория
        /// </summary>
        /// <param name="services">Зависимости</param>
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
    }
}
