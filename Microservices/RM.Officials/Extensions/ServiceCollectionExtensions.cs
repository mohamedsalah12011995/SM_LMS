using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RM.Models;
using RM.Officials.Extensions;
using RM.Officials.Services;
using RM.Officials.UnitOfWorks;

namespace RM.Officials.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
           => services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "RM.Officials", Version = "v1" });
           });


        public static IServiceCollection AddDatabaseContext(this IServiceCollection services
                                         , IConfiguration Configuration)
                => services.AddDbContext<ExternalPortal_v2Context>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DbConnectionString"),
               b => b.MigrationsAssembly(typeof(ExternalPortal_v2Context).Assembly.FullName)));

        public static IServiceCollection RegistrationServices(this IServiceCollection services)
            => services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                       .AddScoped<IUnitOfWork, UnitOfWork>()
                       .AddScoped<IOfficialService, OfficialService>();


    }
}
