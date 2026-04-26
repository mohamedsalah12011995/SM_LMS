using Microsoft.AspNetCore.Mvc;
using RM.Core.CommonDtos;
using RM.Feedbacks.Dtos;

namespace RM.Feedbacks.Services
{
    public interface IAnswerActionService
    {
        public Task<OperationOutput> SaveAnswerAction(FeedbacksAnswerAction RequestedData);
        public Task<OperationOutput> GetFeedbacksEntityItems(Dtos.Feedbacks RequestedData);
        public Task<OperationOutput> GetFeedbacksAnswersStatistics(Dtos.FeedbacksAnswerAction RequestedData);
        public Task<OperationOutput> GetFeedbacksAnswersStatisticsReport(Dtos.FeedbacksAnswerAction RequestedData);
        public Task<OperationOutput> SendEmailFeedbacksStatistics(Dtos.FeedbacksAnswerAction RequestedData);
        public Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord cron);
        public Task<FileStreamResult> ExportFeedbacksStatistics(Dtos.FeedbacksAnswerAction RequestedData);
    }
}
