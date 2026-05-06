
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace RM.GateWay.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection SwaggerGeneration(this IServiceCollection services)=>
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RM.GatewayInner", Version = "v1" });


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

        public static IServiceCollection ConfigureCORSPolicy(this IServiceCollection services, WebApplicationBuilder WebBuilder)
        {
            if (Convert.ToBoolean(WebBuilder.Configuration.GetSection("AppSettings").GetSection("AllowAll").Value))
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("CORSPolicy",
                        builder => builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
                });
               
            }
            else
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("CORSPolicy",
                          builder => builder.WithOrigins(WebBuilder.Configuration.GetSection("AppSettings").GetSection("AllowOrgins").Value.Split(',').ToArray())
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
                });
            }


            return services;
        }

        public static IServiceCollection ConfigureJwtBearer(this IServiceCollection services, WebApplicationBuilder builder, byte[] key)
        {
            if (Convert.ToBoolean(builder.Configuration.GetSection("AppSettings").GetSection("WinAuth").Value) && !Convert.ToBoolean(builder.Configuration.GetSection("AppSettings").GetSection("AllowAll").Value))
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                .AddJwtBearer("ProviderKey", x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.Zero,
                    };
                })
                .AddNegotiate();
            }
            else
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer("ProviderKey", x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.Zero,
                    };
                });
            }
            return services;
        }



    }
}
