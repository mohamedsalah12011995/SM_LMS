using Microsoft.AspNetCore.Mvc;
using RM.Core.CommonDtos;
using RM.Innovations.Dtos;

namespace RM.Innovations.Services
{
    public interface IStatisticsService
    {
        Task<OperationOutput> GetLookUps(Dtos.Statistics RequestedData);
        Task<OperationOutput> GetIdeasStatistics(Dtos.Statistics RequestedData);
        Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord cron);

        Task<OperationOutput> GetIdeasStatisticsReport(Dtos.Statistics RequestedData);
        Task<OperationOutput> SendEmailIdeasStatistics(Dtos.Statistics RequestedData);

        Task<FileStreamResult> ExportIdeasStatistics(Dtos.Statistics RequestedData);
    }
}
