using Notification.Consumer.Interfaces;


namespace Notification.Consumer.Services
{
    public class ConsumerHostedService : BackgroundService
    {

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ConsumerHostedService> _logger;

        public ConsumerHostedService(IServiceScopeFactory scopeFactory, ILogger<ConsumerHostedService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _logger.LogWarning("ConsumerHostedService constructor called.");
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ConsumerHostedService is starting.");

            stoppingToken.Register(() => _logger.LogInformation("ConsumerHostedService is stopping."));
            using (var scope = _scopeFactory.CreateScope())
            {
                var notificationEmailService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                try
                {
                    await notificationEmailService.ReadMessage(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing messages.");
                }
            }
            _logger.LogInformation("ConsumerHostedService has stopped.");
        }


    }



}
