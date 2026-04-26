using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RM.Models;
using RM.Surveys.Extensions;
using RM.Surveys.Services;
using RM.Surveys.UnitOfWorks;
using Surveys.Services.Services;

namespace RM.Surveys.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
           => services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "RM.Surveys", Version = "v1" });
           });


        public static IServiceCollection AddDatabaseContext(this IServiceCollection services
                                         , IConfiguration Configuration)
                => services.AddDbContext<ExternalPortal_v2Context>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DbConnectionString"),
               b => b.MigrationsAssembly(typeof(ExternalPortal_v2Context).Assembly.FullName)));

        public static IServiceCollection RegistrationServices(this IServiceCollection services)
            => services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                       .AddScoped<IUnitOfWork, UnitOfWork>()
                       .AddScoped<ISurveyService, SurveyService>()
                       .AddScoped<IAnswerActionService, AnswerActionService>();
    }
}
