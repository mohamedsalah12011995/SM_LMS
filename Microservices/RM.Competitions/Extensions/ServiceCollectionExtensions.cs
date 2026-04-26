using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RM.Competitions.Repositories;
using RM.Competitions.Services;
using RM.Competitions.UnitOfWorks;


namespace RM.Competitions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
           => services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "RM.Competitions", Version = "v1" });
           });


        public static IServiceCollection AddDatabaseContext(this IServiceCollection services
                                         , IConfiguration Configuration)
                => services.AddDbContext<GardensCompetitionContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DbConnectionString"),
               b => b.MigrationsAssembly(typeof(GardensCompetitionContext).Assembly.FullName)));

        public static IServiceCollection RegistrationServices(this IServiceCollection services)
            => services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                       .AddScoped<IUnitOfWork, UnitOfWork>()
                       .AddScoped<ICompetitionService, CompetitionService>();
    }
}
