using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RM.Exams.Services;
using RM.Exams.UnitOfWorks;
using RM.Models;

namespace RM.Exams.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
           => services.AddSwaggerGen(c =>
           {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "RM.Exams", Version = "v1" });
           });


        public static IServiceCollection AddDatabaseContext(this IServiceCollection services
                                         , IConfiguration Configuration)
                => services.AddDbContext<ExternalPortal_v2Context>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DbConnectionString"),
               b => b.MigrationsAssembly(typeof(ExternalPortal_v2Context).Assembly.FullName)));

        public static IServiceCollection RegistrationServices(this IServiceCollection services)
            => services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                       .AddScoped<IUnitOfWork, UnitOfWork>()
                       .AddScoped<IAnswerActionService, AnswerActionService>()
                       .AddScoped<IExamsService, ExamsService>()
                       .AddScoped<IExamTrainingCoursesService, ExamTrainingCoursesService>();
    }
}
