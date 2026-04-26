using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RM.Models;
using RM.Permits.Extensions;
using RM.Permits.Handlers;
using RM.Permits.Services;
using RM.Permits.UnitOfWorks;

namespace RM.Permits.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
           => services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "RM.Permits", Version = "v1" });
           });


        public static IServiceCollection AddDatabaseContext(this IServiceCollection services
                                         , IConfiguration Configuration)
                => services.AddDbContext<ExternalPortal_v2Context>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DbConnectionString"),
               b => b.MigrationsAssembly(typeof(ExternalPortal_v2Context).Assembly.FullName)));

        public static IServiceCollection RegistrationServices(this IServiceCollection services)
            => services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                       .AddScoped<IUnitOfWork, UnitOfWork>()
                       .AddScoped<IPermitService, PermitService>()
                       .AddScoped<IProjectService, ProjectService>();


    }
}
