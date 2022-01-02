using MathEvent.IdentityServer.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MathEvent.IdentityServer.Email.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureEmail(this IServiceCollection services)
        {
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}
