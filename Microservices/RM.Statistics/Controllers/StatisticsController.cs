using Mapster;
using Microsoft.AspNetCore.Mvc;
using RM.Core.CommonDtos;
using RM.Statistics.Dtos;
using RM.Statistics.Records;
using RM.Statistics.Services;

namespace RM.Statistics.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticService;
        private readonly IStatisticResultService _statisticResultService;

        public StatisticsController(IStatisticService statisticService, IStatisticResultService statisticResultService)
        {
            _statisticService = statisticService;
            _statisticResultService = statisticResultService;
        }

        [HttpPost]
        public async Task<OperationOutput> GetEntitiesStatistics(GetEntitiesStatisticsRecord RequestedData)

             => await _statisticService.GetEntitiesStatistics(RequestedData.Adapt<Dtos.Statistics>());

        [HttpPost]

        public async Task<OperationOutput> SetInteractionStatistics(Dtos.SetStatisticsRequest RequestedData)

            => await _statisticService.SaveInteractionStatistics(RequestedData.ReferenceId, RequestedData.EntityId,
                RequestedData.ItemId, RequestedData.StatisticsType, RequestedData.ItemUrl,RequestedData.value);


        [HttpPost]
        public async Task<OperationOutput> SetHelpfulOrNot(SetHelpfulOrNotRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<Dtos.Statistics>();
            return  await _statisticService.SaveIsHelpful(RequestedData.ReferenceId, RequestedData.EntityId,
                RequestedData.ItemId, RequestedData.IsHelpful, RequestedData.ItemUrl);
        }


        [HttpPost]

        public OperationOutput GetPortalLatestUpdate(GetPortalLatestUpdateRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<Dtos.Statistics>();
            var result = _statisticService.GetPortalLatestUpdate(RequestedData);
            _statisticService.SaveInteractionStatistics(RequestedData.ReferenceId, RequestedData.EntityId,
                                                         null, RequestedData.StatisticsType, null);
            return result;
        }


        [HttpPost]

        public async Task<OperationOutput> GetTotalStatistics(GetTotalStatisticsRecord RequestedData)

            => await _statisticService.GetTotalStatistics(RequestedData.Adapt<Dtos.Statistics>());


        #region ComplaintsStatistics

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetLookUps()
        {
            return await _statisticResultService.GetLookUps();
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetInteractionStatistics(Dtos.Statistics RequestedData)
        {
            return await _statisticResultService.GetInteractionStatistics(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetInteractionStatisticsReport(Dtos.Statistics RequestedData)
        {
            return await _statisticResultService.GetInteractionStatisticsReport(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SendEmailInteractionStatistics(Dtos.Statistics RequestedData)
        {
            return await _statisticResultService.SendEmailInteractionStatistics(RequestedData);
        }


        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord RequestedData)
        {
            return await _statisticResultService.CronJobSendReportsByEmail(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> ExportInteractionStatistics(Dtos.Statistics RequestedData)
        {
            return await _statisticResultService.ExportInteractionStatistics(RequestedData);
        }
        #endregion


    }
}
