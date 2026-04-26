using Mapster;
using RabbitMQ.Core.Dtos;
using RabbitMQ.Core.Extensions;
using RM.Exams.Extensions;
using RM.Exams.Filters;
using Serilog;

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

builder.Services.AddHostedService<CourseScheduleHostedService>();

#region RabbitMQ

builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQConfiguration"));
builder.Services.AddCommonServices(builder.Configuration);

#endregion

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


