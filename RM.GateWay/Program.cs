
using System.Text;
using RM.GateWay.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using RM.GateWay;
using Serilog;


IConfigurationRoot configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
//Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath);
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);


// Add services to the container.

var key = Encoding.ASCII.GetBytes("SecureKeyRequiredforvalidationAdmin");
builder.Services.AddControllers();
builder.Services.SwaggerGeneration()
                .ConfigureCORSPolicy(builder)
                .ConfigureJwtBearer(builder, key)
                .AddAuthorization();
               


var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigureSwagger();
app.UseRouting();
app.UseCors("CORSPolicy");
app.UseAuthMiddleWare();
app.UseAuthentication();
app.UseAuthorization();
app.UseLoggerMiddleWare();
app.ConfigureEndpoints();
app.UseOcelot().Wait();

app.Run();