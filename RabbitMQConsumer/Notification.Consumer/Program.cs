using Notification.Consumer.Extensions;
using Notification.Consumer.Interfaces;
using Notification.Consumer.Services;
using RabbitMQ.Core.Dtos;
using RabbitMQ.Core.Extensions;
using RM.Core.CommonDtos;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext().CreateLogger();

//builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
                 .AddSwaggerGen()
                 .AddCors();


var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

#region RabbitMQ

builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQConfiguration"));
builder.Services.AddCommonServices(builder.Configuration);
builder.Services.AddScoped<INotificationService, NotificationEmailService>();
builder.Services.AddHostedService<ConsumerHostedService>();

#endregion



var app = builder.Build();

app.ConfigureSwagger();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();
app.UseExceptionHandler(opt => { });
app.MapControllers();
app.Run();


