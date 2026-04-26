
using RM.Core.CommonDtos;
using RM.OpenData.Dtos;

namespace RM.OpenData.Services
{
    public interface IOpenDataService
    {
        Task<OperationOutput> RequestOpenData(Dtos.OpenDataRequest RequestedData);
        Task<OperationOutput> OpenDataStats();
        Task<OperationOutput> GetOpenDataRequestList(Dtos.OpenDataRequest RequestedData);
        Task<OperationOutput> GetOpenDataRequestDetails(Dtos.OpenDataRequest RequestedData);
        Task<OperationOutput> GetOpenDataLookups(Dtos.OpenData RequestedData);
        Task<OperationOutput> GetOpenDataStatistics(Dtos.OpenData RequestedData);
        Task<OperationOutput> SaveOpenData(Dtos.OpenData RequestedData);
        Task<OperationOutput> GetOpenDataList(Dtos.OpenData RequestedData);
        Task<OperationOutput> GetOpenDataGroupbyList(Dtos.OpenData RequestedData);
        Task<OperationOutput> GetOpenDataDetails(Dtos.OpenData RequestedData);
        Task<OperationOutput> GetOpenDataByFiledDetails(Dtos.OpenData RequestedData);
        Task<OperationOutput> DeleteOpenData(Dtos.OpenData RequestedData);
        Task<OperationOutput> ModelAction(Dtos.OpenDataRequest RequestedData);
        Task<OperationOutput> GetOpenDataSearchStatistics(Dtos.OpenDataSearchStatistics RequestedData);
        Task SaveOpenDataStatistics(Dtos.OpenData RequestedData);

        Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord cron);
        Task<OperationOutput> SendEmailOpenDataStatistics(OpenDataSearchStatistics RequestedData);
        Task<OperationOutput> GetOpenDataSearchStatisticsReport(OpenDataSearchStatistics RequestedData);
    }
}
