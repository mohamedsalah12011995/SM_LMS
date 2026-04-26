using Microsoft.AspNetCore.Mvc;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Surveys.Dtos;

namespace RM.Surveys.Services
{
    public interface ISurveyService
    {
        #region SURVEY
        public Task<OperationOutput> GetSurveyQuestionLookups(Survey RequestedData);
        public Task<OperationOutput> GetReferences();
        public Task<OperationOutput> GetSurveysList(Survey RequestedData);
        public Task<OperationOutput> SaveSurveyAll(Survey RequestedData);
        public Task<OperationOutput> SaveSurvey(Survey RequestedData);
        public Task<OperationOutput> SaveQuestion(SurveyQuestion RequestedData);
        public Task<OperationOutput> GetSurveyDetails(Survey RequestedData);
        public Task<OperationOutput> GetSurveyQuestions(Survey RequestedData);
        public Task<OperationOutput> SurveyQuestionOrder(Survey RequestedData);
        public Task<OperationOutput> SurveyClone(Survey RequestedData);
        public Task<OperationOutput> ModelActions(Survey RequestedData);
        public Task<OperationOutput> GetSurveysStatistics(Survey RequestedData);
        public Task<OperationOutput> GetSurveyAnswersStatistics(SurveyResult RequestedData);
        public Task<OperationOutput> GetSurveyAnswersStatisticsReport(SurveyResult RequestedData);
        public Task<OperationOutput> GetSurveysWithStatisticsList(Survey RequestedData);
        public FileStreamResult GetPathOfResource(string fileName);

        #endregion

        #region  QUESTIONS
        public Task<OperationOutput> GetQuestionDetails(SurveyQuestion RequestedData);
        public Task<OperationOutput> QuestionModelActions(SurveyQuestion RequestedData);
        public Task<OperationOutput> GetAllQuestionsList(SurveyQuestion RequestedData);
        public Task<OperationOutput> QuestionsGlobalList(List<SurveyQuestion> RequestedData);
        public Task<OperationOutput> QuestionsDeleteList(List<SurveyQuestion> RequestedData);
        public Task<OperationOutput> QuestionsActivationList(List<SurveyQuestion> RequestedData);

        #endregion

        #region SendEmail
        public Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord cron);
        public Task<OperationOutput> SendEmailSurveysStatistics(SurveyResult RequestedData);
        public Task<OperationOutput> GetSurveyResultLookUps();
        public Task<OperationOutput> UpdateSurveyResultLookUps(List<MajorLookups> RequestedData);
        #endregion

    }
}
