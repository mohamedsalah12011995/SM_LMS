using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RM.ScientificLetters.Services;
using RM.ScientificLetters.UnitOfWorks;
using RM.Models;

namespace RM.ScientificLetters.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection services)
          => services.AddSwaggerGen(c =>
          {
              c.SwaggerDoc("v1", new OpenApiInfo { Title = "RM.Advertisements", Version = "v1" });

              c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
              {
                  Name = "Authorization",
                  Type = SecuritySchemeType.Http,
                  Scheme = "bearer",
                  BearerFormat = "JWT",
                  In = ParameterLocation.Header,
                  Description = "Enter 'Bearer' [space] and then your valid token.\nExample: Bearer eyJhbGciOiJIUzI1NiIs..."
              });

              c.AddSecurityRequirement(new OpenApiSecurityRequirement
         {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
       });
          });


        public static IServiceCollection AddDatabaseContext(this IServiceCollection services
                                         , IConfiguration Configuration)
                =>services.AddDbContext<ExternalPortal_v2Context>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DbConnectionString"),
               b => b.MigrationsAssembly(typeof(ExternalPortal_v2Context).Assembly.FullName)));

        public static IServiceCollection RegistrationServices(this IServiceCollection services)
            => services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                       .AddScoped<IUnitOfWork, UnitOfWork>()
                       .AddScoped<IScientificLettersService, ScientificLettersService>();
    }
}
