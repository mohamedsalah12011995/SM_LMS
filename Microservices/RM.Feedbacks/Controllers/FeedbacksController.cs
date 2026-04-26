using Microsoft.AspNetCore.Mvc;
using RM.Feedbacks.Services;
using RM.Feedbacks.Dtos;
using RM.Core.CommonDtos;

namespace RM.Feedbacks.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {

        private readonly IFeedbacksService _FeedbacksService;
        private readonly IAnswerActionService _ActionAnswerService;
        public FeedbacksController(IFeedbacksService FeedbacksService, IAnswerActionService ActionAnswerService)
        {
            _FeedbacksService = FeedbacksService;
            _ActionAnswerService = ActionAnswerService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetFeedbacksLookups(Dtos.Feedbacks RequestedData)
        {
            return await _FeedbacksService.GetFeedbacksLookups(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetFeedbacksList(Dtos.Feedbacks RequestedData)
        {
            return await _FeedbacksService.GetFeedbacksList(RequestedData);
        }
        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetFeedbacksDetails(Dtos.Feedbacks RequestedData)
        {
            return await _FeedbacksService.GetFeedbacksDetails(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveFeedbacks(Dtos.Feedbacks RequestedData)
        {
            return await _FeedbacksService.SaveFeedbacks(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> FeedbacksModelActions(Dtos.Feedbacks RequestedData)
        {
            return await _FeedbacksService.FeedbacksModelActions(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveAnswerAction(Dtos.FeedbacksAnswerAction RequestedData)
        {
            return await _ActionAnswerService.SaveAnswerAction(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetFeedbacksEntityItems(Dtos.Feedbacks RequestedData)
        {
            return await _ActionAnswerService.GetFeedbacksEntityItems(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetFeedbacksAnswersStatistics(Dtos.FeedbacksAnswerAction RequestedData)
        {
            return await _ActionAnswerService.GetFeedbacksAnswersStatistics(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetFeedbacksAnswersStatisticsReport(Dtos.FeedbacksAnswerAction RequestedData)
        {
            return await _ActionAnswerService.GetFeedbacksAnswersStatisticsReport(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SendEmailFeedbacksStatistics(Dtos.FeedbacksAnswerAction RequestedData)
        {
            return await _ActionAnswerService.SendEmailFeedbacksStatistics(RequestedData);
        }


        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord RequestedData)
        {
            return await _ActionAnswerService.CronJobSendReportsByEmail(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> ExportFeedbacksStatistics(Dtos.FeedbacksAnswerAction RequestedData)
        {
            return await _ActionAnswerService.ExportFeedbacksStatistics(RequestedData);
        }

    }
}
