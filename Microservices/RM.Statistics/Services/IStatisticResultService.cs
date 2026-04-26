
using Microsoft.AspNetCore.Mvc;
using RM.Core.CommonDtos;
using RM.Statistics.Dtos;

namespace RM.Statistics.Services
{
    public interface IStatisticResultService
    {
        Task<OperationOutput> GetLookUps();
        Task<OperationOutput> GetInteractionStatistics(Dtos.Statistics RequestedData);
        Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord cron);
        Task<OperationOutput> GetInteractionStatisticsReport(Dtos.Statistics RequestedData);
        Task<OperationOutput> SendEmailInteractionStatistics(Dtos.Statistics RequestedData);
        Task<FileStreamResult> ExportInteractionStatistics(Dtos.Statistics RequestedData);
    }
}
