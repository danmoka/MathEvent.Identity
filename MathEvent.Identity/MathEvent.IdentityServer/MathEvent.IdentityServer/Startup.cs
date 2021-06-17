using MathEvent.IdentityServer.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MathEvent.IdentityServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureConnection(Configuration);
            services.ConfigureIndentity();
            services.ConfigureIdentityServer(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder.WithOrigins(Configuration.GetSection("Origins").Get<string[]>());
                builder.AllowCredentials();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });

            app.UseRouting();

            app.UseIdentityServer();
        }
    }
}
