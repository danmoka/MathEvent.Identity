using MathEvent.IdentityServer.Authorization.Extensions;
using MathEvent.IdentityServer.Database.Extensions;
using MathEvent.IdentityServer.Email.Extensions;
using MathEvent.IdentityServer.Extensions;
using MathEvent.IdentityServer.Mappers.Extensions;
using MathEvent.IdentityServer.Repositories.Extensions;
using MathEvent.IdentityServer.Services.Extensions;
using MathEvent.IdentityServer.Validation.Extensions;
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

        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureIndentity();
            services.ConfigureDbConnection(Configuration);
            services.ConfigureAuthentication(Configuration, Environment);
            services.ConfigureAuthorization(Configuration);
            services.ConfigureRepositoryWrapper();
            services.ConfigureEntityServices();
            services.ConfigureAuthorizationHandlers();
            services.ConfigureEmail();
            services.ConfigureMapper();
            services.ConfigureValidation();
            services.ConfigureControllers();
            services.ConfigureIdentityServer(Environment, Configuration);
            services.ConfigureOpenApi();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(builder =>
                {
                    builder.WithOrigins(Configuration.GetSection("Origins").Get<string[]>());
                    builder.AllowCredentials();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
                app.UseHttpsRedirection();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MathEvent.IdentityServer.Api v1"));
            }
            else if (env.IsProduction())
            {
                app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization("MathEventIdentityServer.Api");
            });
        }
    }
}
