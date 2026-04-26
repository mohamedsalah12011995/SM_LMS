


using CronJobService.Services;
using RM.Core.Helpers;

namespace CronJobService.CronJob
{
    public class HostedService : IHostedService, IDisposable
    {
        private Timer _timerEveryDay;
        private Timer _timerEveryWeek;
        private Timer _timerEveryMonth;
        private Timer _timerEveryHalfDay;

        List<int> QuarterMonthes;
        private readonly IServiceScopeFactory _scopeFactory;

        public HostedService(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            QuarterMonthes = new List<int> { 3,6,9,12};
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timerEveryDay = new Timer(SendEveryDay, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            _timerEveryWeek = new Timer(SendEveryWeek, null, TimeSpan.Zero, TimeSpan.FromDays(7));
            _timerEveryMonth = new Timer(SendEveryMonth, null, TimeSpan.Zero, TimeSpan.FromDays(30));
            _timerEveryHalfDay = new Timer(SendEveryHalfDay, null, TimeSpan.Zero, TimeSpan.FromHours(12));
            return Task.CompletedTask;
        }

        private async void SendEveryDay(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _CronSettingsService = scope.ServiceProvider.GetRequiredService<ICronSettingsService>();

                await _CronSettingsService.DoWork((int)Enums.CronType.EveryDay);
            }
        }

        private async void SendEveryWeek(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _CronSettingsService = scope.ServiceProvider.GetRequiredService<ICronSettingsService>();

                await _CronSettingsService.DoWork((int)Enums.CronType.EveryWeek);
            }
        }

        private async void SendEveryMonth(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _CronSettingsService = scope.ServiceProvider.GetRequiredService<ICronSettingsService>();

                await _CronSettingsService.DoWork((int)Enums.CronType.EveryMonth);
            }

            if (QuarterMonthes.Contains(DateTime.Today.Month))
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _CronSettingsService = scope.ServiceProvider.GetRequiredService<ICronSettingsService>();
                    await _CronSettingsService.DoWork((int)Enums.CronType.EveryQuaters);
                }
            }
        }



        private async void SendEveryHalfDay(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _CronSettingsService = scope.ServiceProvider.GetRequiredService<ICronSettingsService>();

                await _CronSettingsService.DoWork((int)Enums.CronType.WhenFinishToDate);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {

            _timerEveryDay?.Change(Timeout.Infinite, 0);
            _timerEveryWeek?.Change(Timeout.Infinite, 0);
            _timerEveryMonth?.Change(Timeout.Infinite, 0);
            _timerEveryHalfDay?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timerEveryDay?.Dispose();
            _timerEveryWeek?.Dispose();
            _timerEveryMonth?.Dispose();
            _timerEveryHalfDay?.Dispose();
        }
    }
}
