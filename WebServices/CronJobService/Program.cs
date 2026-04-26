using CronJobService.CronJob;
using CronJobService.Filters;
using CronJobService.Extensions;
using Serilog;
using Mapster;

var builder = WebApplication.CreateBuilder(args);
//var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

// Add services to the container.
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext().CreateLogger();

//builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddMemoryCache();

builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor()
.AddSwaggerGen()
.AddDatabaseContext(builder.Configuration)
.RegistrationServices()
.AddCors()
.AddMapster();

//if (Convert.ToBoolean(builder.Configuration.GetSection("AppSettings").GetSection("RunCronJob").Value))
    builder.Services.AddHostedService<HostedService>();


var app = builder.Build();

// Configure the HTTP request pipeline.


app.ConfigureDirectoryResources();
app.ConfigureSwagger();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();
app.UseExceptionHandler(opt => { });
app.MapControllers();
app.Run();


