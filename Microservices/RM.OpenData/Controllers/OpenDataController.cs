using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using RM.Core.CommonDtos;
using RM.OpenData.Dtos;
using RM.OpenData.Records;
using RM.OpenData.Services;

namespace RM.OpenData.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OpenDataController
    {
        private readonly IOpenDataService _openDataService;
        private readonly IOpenDataTempService _openDataTempService;

        public OpenDataController(IOpenDataService openDataService, IOpenDataTempService openDataTempService)
        {
            _openDataService = openDataService;
            _openDataTempService = openDataTempService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> OpenDataRequest(OpenDataRequestRecord RequestedData)
        {
             return await _openDataService.RequestOpenData(RequestedData.Adapt<Dtos.OpenDataRequest>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> OpenDataStats()
        {   
            return await _openDataService.OpenDataStats();
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetOpenDataRequestList(GetOpenDataRequestListRecord RequestedData)
        {
           return await _openDataService.GetOpenDataRequestList(RequestedData.Adapt<Dtos.OpenDataRequest>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> GetOpenDataRequestDetails(GetOpenDataRequestDetailsRecord RequestedData)
        {
           return await _openDataService.GetOpenDataRequestDetails(RequestedData.Adapt<Dtos.OpenDataRequest>());
        }

        
        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetOpenDataLookups(GetOpenDataLookupsRecord RequestedData)
        {
           return await _openDataService.GetOpenDataLookups(RequestedData.Adapt<Dtos.OpenData>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetOpenDataStatistics(GetOpenDataStatisticsRecord RequestedData)
        {
           return await _openDataService.GetOpenDataStatistics(RequestedData.Adapt<Dtos.OpenData>());
        }

        
        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveOpenData(SaveOpenDataRecord RequestedData)
        {
           return await _openDataService.SaveOpenData(RequestedData.Adapt<Dtos.OpenData>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetOpenDataList(GetOpenDataListRecord RequestedData)
        {
           return await _openDataService.GetOpenDataList(RequestedData.Adapt<Dtos.OpenData>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetOpenDataGroupbyList(GetOpenDataGroupbyListRecord RequestedData)
        {
           return await _openDataService.GetOpenDataGroupbyList(RequestedData.Adapt<Dtos.OpenData>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetOpenDataDetails(GetOpenDataDetailsRecord RequestedData)
        {
           return await _openDataService.GetOpenDataDetails(RequestedData.Adapt<Dtos.OpenData>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetOpenDataByFiledDetails(GetOpenDataByFiledDetailsRecord RequestedData)
        {
            return await _openDataService.GetOpenDataByFiledDetails(RequestedData.Adapt<Dtos.OpenData>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> DeleteOpenData(DeleteOpenDataRecord RequestedData)
        {
           return await _openDataService.DeleteOpenData(RequestedData.Adapt<Dtos.OpenData>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetOpenDataTempDetails(GetOpenDataTempDetailsRecord RequestedData)
        {
           return await _openDataTempService.GetOpenDataTempDetails(RequestedData.Adapt<Dtos.OpenData>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetOpenDataTempByFiledDetails(GetOpenDataTempByFiledDetailsRecord RequestedData)
        {
           return await _openDataTempService.GetOpenDataTempByFiledDetails(RequestedData.Adapt<Dtos.OpenData>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> DeleteOpenDataTemp(List<DeleteOpenDataTempRecord> RequestedData)
        {
           return await _openDataTempService.DeleteOpenDataTemp(RequestedData.Adapt<List<Dtos.OpenData>>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> ConfirmOpenDataTemp(List<ConfirmOpenDataTempRecord> RequestedData)
        {
           return await _openDataTempService.ConfirmOpenDataTemp(RequestedData.Adapt<List<Dtos.OpenData>>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> ConfirmAllOpenDataTemp(ConfirmAllOpenDataTempRecord RequestedData)
        {
           return await _openDataTempService.ConfirmAllOpenDataTemp(RequestedData.Adapt<Dtos.OpenData>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveOpenDataTemp(SaveOpenDataTempRecord RequestedData)
        {
           return await _openDataTempService.SaveOpenDataTemp(RequestedData.Adapt<Dtos.OpenData>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetOpenDataTempList(GetOpenDataTempListRecord RequestedData)
        {
           return await _openDataTempService.GetOpenDataTempList(RequestedData.Adapt<Dtos.OpenData>());
        }


        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> Delete(DeleteRecord RequestedData)
        {
            return await _openDataService.ModelAction(RequestedData.Adapt<Dtos.OpenDataRequest>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SyncWithOpenDataTemp(RequestRecords RequestedData)
        {
           return await _openDataTempService.SyncWithOpenDataTemp(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SyncWithOpenData(List<SyncWithOpenDataRecord> RequestedDataList)
        {
           return await _openDataTempService.SyncWithOpenData(RequestedDataList.Adapt<List<Dtos.OpenData>>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SyncAllWithOpenData(SyncAllWithOpenDataRecord RequestedData)
        {
           return await _openDataTempService.SyncAllWithOpenData(RequestedData.Adapt<Dtos.OpenData>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetOpenDataSearchStatistics(GetOpenDataSearchStatisticsRecord RequestedData)
        {
           return await _openDataService.GetOpenDataSearchStatistics(RequestedData.Adapt<Dtos.OpenDataSearchStatistics>());
        }

        [HttpPost]
        public async Task<OperationOutput> GetOpenDataSearchStatisticsReport(OpenDataSearchStatistics RequestedData)
        {
            return await _openDataService.GetOpenDataSearchStatisticsReport(RequestedData);
        }

        [HttpPost]
        public async Task<OperationOutput> SendEmailOpenDataStatistics(OpenDataSearchStatistics RequestedData)
        {
           return await _openDataService.SendEmailOpenDataStatistics(RequestedData);
        }

        [HttpPost]
        public async Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord RequestedData)
        {
           return await _openDataService.CronJobSendReportsByEmail(RequestedData);
        }
    }
}
