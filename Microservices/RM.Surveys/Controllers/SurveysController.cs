using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Surveys.Dtos;
using RM.Surveys.Records;
using RM.Surveys.Services;
using Surveys.Services.Services;
using System.Collections.Generic;
using static RM.Core.Helpers.Enums;


namespace RM.Surveys.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SurveysController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        private readonly IAnswerActionService _answerActionService;

        public SurveysController(ISurveyService surveysService, IAnswerActionService answerActionService)
        {
            _surveyService = surveysService;
            _answerActionService = answerActionService;
        }

        [HttpPost]
        public async Task<OperationOutput> GetSurveyQuestionLookups(GetSurveyQuestionLookupsRecord RequestedData)
            => await _surveyService.GetSurveyQuestionLookups(RequestedData.Adapt<Survey>());


        [HttpPost]
        public async Task<OperationOutput> GetSurveysList(GetSurveysListRecord RequestedData)
            => await _surveyService.GetSurveysList(RequestedData.Adapt<Survey>());

        [HttpPost]
        public async Task<OperationOutput> SaveSurveyAll(SaveSurveyAllRecord RequestedData)
            => await _surveyService.SaveSurveyAll(RequestedData.Adapt<Survey>());

        [HttpPost]
        public async Task<OperationOutput> SaveSurvey(SaveSurveyRecord RequestedData)
            => await _surveyService.SaveSurvey(RequestedData.Adapt<Survey>());


        [HttpPost]
        public async Task<OperationOutput> SaveQuestion(SaveQuestionRecord RequestedData)
             => await _surveyService.SaveQuestion(RequestedData.Adapt<SurveyQuestion>());


        [HttpPost]
        public async Task<OperationOutput> GetQuestionDetails(GetQuestionDetailsRecord RequestedData)
               => await _surveyService.GetQuestionDetails(RequestedData.Adapt<SurveyQuestion>());


        [HttpPost]
        public async Task<OperationOutput> GetSurveyDetails(GetSurveyDetailsRecord RequestedData)
            => await _surveyService.GetSurveyDetails(RequestedData.Adapt<Survey>());


        [HttpPost]
        public async Task<OperationOutput> QuestionModelActions(QuestionModelActionsRecord RequestedData)
            => await _surveyService.QuestionModelActions(RequestedData.Adapt<SurveyQuestion>());

        [HttpPost]
        public async Task<OperationOutput> ModelActions(ModelActionsRecord RequestedData)
             => await _surveyService.ModelActions(RequestedData.Adapt<Survey>());


        [HttpPost]
        public async Task<OperationOutput> SaveAnswerAction(SaveAnswerActionRecord RequestedData)
            => await _answerActionService.SaveAnswerAction(RequestedData.Adapt<SurveyAnswerAction>());


        [HttpPost]
        public async Task<OperationOutput> GetSurveysStatistics(GetSurveysStatisticsRecord RequestedData)
            => await _surveyService.GetSurveysStatistics(RequestedData.Adapt<Survey>());


        [HttpPost]
        public Task<OperationOutput> GetSurveyAnswersStatistics(GetSurveyAnswersStatisticsRecord RequestedData)
              => _surveyService.GetSurveyAnswersStatistics(RequestedData.Adapt<SurveyResult>());

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetSurveyAnswersStatisticsReport(Dtos.SurveyResult RequestedData)
                => await _surveyService.GetSurveyAnswersStatisticsReport(RequestedData);



        [HttpPost]
        public async Task<OperationOutput> GetReferences()
            => await _surveyService.GetReferences();

        [HttpPost]
        public async Task<OperationOutput> GetSurveyQuestions(GetSurveyQuestionsRecord RequestedData)
            => await _surveyService.GetSurveyQuestions(RequestedData.Adapt<Survey>());


        [HttpPost]
        public async Task<OperationOutput> SurveyQuestionOrder(SurveyQuestionOrderRecord RequestedData)
              => await _surveyService.SurveyQuestionOrder(RequestedData.Adapt<Survey>());


        [HttpPost]
        public async Task<OperationOutput> SurveyClone(SurveyCloneRecord RequestedData)
            => await _surveyService.SurveyClone(RequestedData.Adapt<Survey>());



        [HttpPost]
        public async Task<OperationOutput> GetAllQuestionsList(GetAllQuestionsListRecord RequestedData)
            => await _surveyService.GetAllQuestionsList(RequestedData.Adapt<SurveyQuestion>());


        [HttpPost]
        public async Task<OperationOutput> QuestionsGlobalList(List<QuestionsGlobalListRecord> RequestedData)
            => await _surveyService.QuestionsGlobalList(RequestedData.Adapt<List<SurveyQuestion>>());



        [HttpPost]
        public async Task<OperationOutput> QuestionsDeleteList(List<QuestionsDeleteListRecord> RequestedData)
            => await _surveyService.QuestionsDeleteList(RequestedData.Adapt<List<SurveyQuestion>>());

        [HttpPost]

        public async Task<OperationOutput> QuestionsActivationList(List<QuestionsActivationListRecord> RequestedData)
            => await _surveyService.QuestionsActivationList(RequestedData.Adapt<List<SurveyQuestion>>());

        [HttpPost]
        public async Task<OperationOutput> GetSurveysWithStatisticsList(GetSurveysWithStatisticsListRecord RequestedData)
            => await _surveyService.GetSurveysWithStatisticsList(RequestedData.Adapt<Survey>());

        [HttpPost]
        public async Task<OperationOutput> SendEmailSurveysStatistics(SendEmailSurveysStatisticsRecord RequestedData)
            => await _surveyService.SendEmailSurveysStatistics(RequestedData.Adapt<SurveyResult>());

        [HttpPost]
        public async Task<OperationOutput> GetSurveyResultLookUps()
            => await _surveyService.GetSurveyResultLookUps();

        [HttpPost]
        public async Task<OperationOutput> UpdateSurveyResultLookUps(List<MajorLookups> RequestedData)
            => await _surveyService.UpdateSurveyResultLookUps( RequestedData);

        [HttpPost]
        public async Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord RequestedData)
            => await _surveyService.CronJobSendReportsByEmail(RequestedData);

        [AllowAnonymous]
        [HttpGet]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
             => _surveyService.GetPathOfResource(fileName);




    }
}
