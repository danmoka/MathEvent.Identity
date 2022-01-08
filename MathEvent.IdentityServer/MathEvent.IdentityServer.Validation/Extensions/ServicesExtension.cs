using MathEvent.IdentityServer.Contracts.Validation.User;
using MathEvent.IdentityServer.Validation.User;
using Microsoft.Extensions.DependencyInjection;

namespace MathEvent.IdentityServer.Validation.Extensions
{
    /// <summary>
    /// Расширение IServiceCollection
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Конфигурация валидации
        /// </summary>
        /// <param name="services">Сервисы</param>
        public static void ConfigureValidation(this IServiceCollection services)
        {
            services.AddTransient<IMathEventIdentityUserCreateModelValidator, MathEventIdentityUserCreateModelValidator>();
            services.AddTransient<IMathEventIdentityUserUpdateModelValidator, MathEventIdentityUserUpdateModelValidator>();
            services.AddTransient<IForgotPasswordModelValidator, ForgotPasswordModelValidator>();
            services.AddTransient<IForgotPasswordResetModelValidator, ForgotPasswordResetModelValidator>();
            services.AddTransient<IMathEventIdentityUserRoleModelValidator, MathEventIdentityUserRoleModelValidator>();
            services.AddTransient<UserValidationUtils>();
        }
    }
}
