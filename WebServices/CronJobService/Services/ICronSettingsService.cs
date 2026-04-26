
using CronJobService.Dtos;

namespace CronJobService.Services
{
    public interface ICronSettingsService
    {
        Task<OperationOutput> DoWork(int CronType);
    }
}
