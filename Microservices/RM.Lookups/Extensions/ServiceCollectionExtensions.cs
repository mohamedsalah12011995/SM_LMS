using RM.Lookups.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RM.Lookups.UnitOfWorks;
using RM.Models;

namespace RM.Lookups.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
           => services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "RM.Lookups", Version = "v1" });
           });


        public static IServiceCollection AddDatabaseContext(this IServiceCollection services
                                         , IConfiguration Configuration)
                =>services.AddDbContext<ExternalPortal_v2Context>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DbConnectionString"),
               b => b.MigrationsAssembly(typeof(ExternalPortal_v2Context).Assembly.FullName)));

        public static IServiceCollection RegistrationServices(this IServiceCollection services)
            => services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                       .AddScoped<IUnitOfWork, UnitOfWork>()
                       .AddScoped<IRecommendionService, RecommendionService>()
                       .AddScoped<ICronSettingsService, CronSettingsService>()
                       .AddScoped<ILookupsService, LookupsService>();
    }
}
