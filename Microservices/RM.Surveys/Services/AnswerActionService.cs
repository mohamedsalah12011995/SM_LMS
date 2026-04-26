using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Surveys.Dtos;
using RM.Surveys.UnitOfWorks;
using System.Text.Json;
using static RM.Surveys.Dtos.OperationOutput;


namespace RM.Surveys.Services
{

    public class AnswerActionService : BaseService, IAnswerActionService
    {
        JsonSerializerOptions jsonOptions = null;
        private readonly IUnitOfWork _unitOfWork;
        private static string imagesGetPath = string.Empty;
        private static string filesGetPath = string.Empty;

        public AnswerActionService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            imagesGetPath = filesGetPath = Strings.HandleGetResourcesPath(IsLocal, GetPath, IntranetGetPath);

            SetDefaultValueToJsonSerializerOptions();


        }

        public async Task<OperationOutput> SaveAnswerAction(SurveyAnswerAction RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var surveyAnswerAction = new Models.SurveyAnswerAction();

            var survey = await _unitOfWork.Surveys.GetByIdAsync(RequestedData._surveyId.Value);

            if (survey is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            surveyAnswerAction.CreatedBy = RequestOwner.Id;
            surveyAnswerAction.CreatedDate = TransactionDate;
            surveyAnswerAction.SurveyId = RequestedData._surveyId;

            foreach (var ans in RequestedData.SurveyQuestionAnswers)
            {
                var dbQuestionAnswer = new Models.SurveyQuestionAnswer();

                switch (ans.QuestionType)
                {
                    case SurveyInputTypes.ImageType:
                        dbQuestionAnswer.Text = $"{imagesGetPath}/{ConvertImageBase64ToString(ans.Text)}";
                        break;
                    case SurveyInputTypes.FileType:
                        dbQuestionAnswer.Text = $"{filesGetPath}/{ConvertFileBase64ToString(ans.Text)}";
                        break;
                    default:
                        dbQuestionAnswer.Text = ans.Text;
                        break;
                }

                dbQuestionAnswer.Value = ans.Value;
                dbQuestionAnswer.Notes = ans.Notes;
                dbQuestionAnswer.DataSourceId = ans._dataSourceId;
                dbQuestionAnswer.QuestionId = ans._questionId;
                dbQuestionAnswer.SurveyAnswerActionId = surveyAnswerAction.Id;
                surveyAnswerAction.SurveyQuestionAnswers.Add(dbQuestionAnswer);
            }

            _unitOfWork.SurveyAnswerActions.Add(surveyAnswerAction);
            _unitOfWork.Complete();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.Id, surveyAnswerAction.Id));
        }

        #region HELPER METHODS
        private void SetDefaultValueToJsonSerializerOptions()
        {
            jsonOptions = new JsonSerializerOptions();
            jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            jsonOptions.PropertyNameCaseInsensitive = true;
        }

        private string ConvertImageBase64ToString(string fieldText)
        {
            if (!string.IsNullOrEmpty(fieldText) && Files.GetBase64FileSizeMb(fieldText) > FileSizeMb)
                return null;
            else if (!string.IsNullOrEmpty(fieldText))
            {
                return !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(fieldText)
                            ? Images.SaveSingleImageOnServer(fieldText, null, ImagesSavePath, false) : null;
            }
            return null;
        }

        private string ConvertFileBase64ToString(string fieldText)
        {

            if (!string.IsNullOrEmpty(fieldText) && Files.GetBase64FileSizeMb(fieldText) > FileSizeMb)
                return null;
            if (!Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(fieldText))
            {
                var FileName = Strings.GenerateGUID() + ".pdf";
                return Files.SaveBase64FileToServer(FileName, fieldText, FilesSavePath);

            }

            return null;
        }

        #endregion

    }
}
