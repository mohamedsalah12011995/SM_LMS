using RM.Models;


public class CourseScheduleHostedService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CourseScheduleHostedService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(7));
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ExternalPortal_v2Context>();

            var schedulesToClose = context.TrainingCourseSchedule
                .Where(cs => cs.EndDate.Date < DateTime.Now.Date && cs.IsClosed == false)
                .ToList();

            foreach (var schedule in schedulesToClose)
            {
                schedule.IsClosed = true;
                schedule.IsActive = false;
            }

            context.SaveChanges();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
