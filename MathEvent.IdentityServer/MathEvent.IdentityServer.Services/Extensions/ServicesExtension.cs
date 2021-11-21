using MathEvent.IdentityServer.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MathEvent.IdentityServer.Services.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureEntityServices(this IServiceCollection services)
        {
            services.AddTransient<IMathEventIdentityUserService, MathEventIdentityUserService>();
        }
    }
}
