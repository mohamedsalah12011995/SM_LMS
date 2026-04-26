


using IntegrationService.Extensions;
using IntegrationService.Records;
using RM.Core.CommonDtos;


var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

builder.Services.AddMemoryCache();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor()
.AddSwaggerGen()
.RegistrationServices()
.AddCors();

var HarsPInfoConfig = builder.Configuration.GetSection("HarsPInfoConfiguration").Get<PInfoConfiguration>();
builder.Services.AddSingleton(HarsPInfoConfig);

var HarsSMSConfig = builder.Configuration.GetSection("HarsSMSConfiguration").Get<SMSConfiguration>();
builder.Services.AddSingleton(HarsSMSConfig);

var NafathConfiguration = builder.Configuration.GetSection("NafathConfiguration").Get<NafathConfiguration>();
builder.Services.AddSingleton(NafathConfiguration);

var app = builder.Build();

// Configure the HTTP request pipeline.


app.ConfigureSwagger();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();
//app.UseExceptionHandler(opt => { });

app.MapControllers();
app.Run();

