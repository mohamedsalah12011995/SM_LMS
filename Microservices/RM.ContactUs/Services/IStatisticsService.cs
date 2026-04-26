using Microsoft.AspNetCore.Mvc;
using RM.Core.CommonDtos;
using RM.ContactUs.Dtos;

namespace RM.ContactUs.Services
{
    public interface IStatisticsService
    {
        Task<OperationOutput> GetLookUps(Dtos.Statistics RequestedData);
        Task<OperationOutput> GetSuggestionsAndComplaintsStatistics(Dtos.Statistics RequestedData);
        Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord cron);

        Task<OperationOutput> GetSuggestionsAndComplaintsStatisticsReport(Dtos.Statistics RequestedData);
        Task<OperationOutput> SendEmailSuggestionsAndComplaintsStatistics(Dtos.Statistics RequestedData);

        Task<FileStreamResult> ExportSuggestionsAndComplaintsStatistics(Dtos.Statistics RequestedData);
    }
}
