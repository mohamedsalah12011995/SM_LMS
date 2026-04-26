using Common;
using Mapster;
using MapsterMapper;
using RM.Articles.Extensions;
using RM.Articles.Filters;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

// Add services to the container.
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext().CreateLogger();

//builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor()
.AddSwaggerGen()
.AddDatabaseContext(builder.Configuration)
.RegistrationServices()
.AddCors()
//.AddSingleton(new TypeAdapterConfig())
//.AddScoped<IMapper, ServiceMapper>()
.AddMapster();

var Redis = config.GetSection("RedisConfiguration").Get<RedisConfiguration>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = Redis.Server;
    options.InstanceName = "RM_Net8"; // Optional prefix for keys
});

builder.Services.AddSingleton(Redis);

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

