using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PDFService.Services;

namespace PDFService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
           => services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "RM.PDFService", Version = "v1" });
           });


        public static IServiceCollection RegistrationServices(this IServiceCollection services)
            => services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                       .AddScoped<IPDFServices, PDFServices>();
    }
}
