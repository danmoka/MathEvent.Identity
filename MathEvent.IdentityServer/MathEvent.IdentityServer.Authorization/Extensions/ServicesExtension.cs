using MathEvent.IdentityServer.Authorization.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace MathEvent.IdentityServer.Authorization.Extensions
{
    public static class ServiceExtension
    {
        /// <summary>
        /// Настройка обработчиков авторизации
        /// </summary>
        /// <param name="services">Зависимости</param>
        public static void ConfigureAuthorizationHandlers(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, MathEventIdentityUsersAuthorizationCrudHandler>();
        }
    }
}
