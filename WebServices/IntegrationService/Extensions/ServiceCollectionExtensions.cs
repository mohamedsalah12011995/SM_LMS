using IntegrationService.Services;
using Microsoft.OpenApi.Models;


namespace IntegrationService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
           => services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "IntegrationService", Version = "v1" });
           });


        public static IServiceCollection RegistrationServices(this IServiceCollection services)
            => services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                       .AddScoped<IPInfoService, PInfoService>()
                       .AddScoped<IActiveDirectoryService, ActiveDirectoryService>()
                       .AddScoped<ISMSService, SMSService>()
                       .AddScoped<ITestService, TestService>()
                       .AddScoped<INafathService, NafathService>();
    }
}
