using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using EmailService.Services;

namespace EmailService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
           => services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "RM.EmailService", Version = "v1" });
           });


        public static IServiceCollection RegistrationServices(this IServiceCollection services)
            => services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                       .AddScoped<IEmailServices, EmailServices>();
    }
}
