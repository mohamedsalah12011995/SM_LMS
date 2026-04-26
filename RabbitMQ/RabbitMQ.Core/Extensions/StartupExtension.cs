using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Core.Services;

namespace RabbitMQ.Core.Extensions
{
    public static class StartupExtension
    {
        public static void AddCommonServices(this IServiceCollection services, IConfiguration configuration)
              => services.AddSingleton<IRabbitMqService, RabbitMqService>();

    }
}
