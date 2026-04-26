using Microsoft.OpenApi.Models;

using Notification.Consumer.Extensions;
using Notification.Consumer.Interfaces;
using Notification.Consumer.Services;

namespace Notification.Consumer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
           => services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "Notification", Version = "v1" });
           });


        public static IServiceCollection RegistrationServices(this IServiceCollection services)
            => services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                       .AddScoped<INotificationService, NotificationEmailService>();
    }
}
