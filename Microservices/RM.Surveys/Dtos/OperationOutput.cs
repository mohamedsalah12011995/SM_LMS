using RM.Core.Helpers;
using static RM.Core.Helpers.Enums;

namespace RM.Surveys.Dtos
{
    public class OperationOutput : ApplicationOperation.ResponseOutput
    {

        public class OperationOutputKeys
        {
            public const string Id = "Id";
            public const string SurveysEntity = "SurveysEntity";
            public const string SurveyQuesionEntity = "SurveyQuesionEntity";
            public const string EntityID = "EntityID";
            public const string Pagination = "Pagination";

            public const string NotExpiredServeys = "NotExpiredServeys";
            public const string ExpiredServeys = "ExpiredServeys";
            public const string AnswersCount = "AnswersCount";
            public const string SurveyResult = "SurveyResult";
            public const string SurveysList = "SurveysList";
            public const string TypesList = "TypesList";
            public const string SurveyAnswerCount = "SurveyAnswerCount";
            public const string References = "References";
            public const string GroupQuestion = "GroupQuestion";
            public const string SurveyGroupResult = "SurveyGroupResult";

            public const string SurveyStatistics = "SurveyStatistics";
            public const string SurveyAnswers = "SurveyAnswers";
            public const string GroupSurveyAnswers = "GroupSurveyAnswers";
            public const string RatingQuestsResult = "RatingQuestsResult";
            public const string RangeQuestsResult = "RangeQuestsResult";
            public const string SurveyThemes = "SurveyThemes";
            public const string SurveyQuestionEntity = "SurveyQuestionEntity";
            public const string SurveyStatisticRates = "SurveyStatisticRates";
            public const string ResultLookUps = "ResultLookUps";
            public const string SurveyRecommendions = "SurveyRecommendions";
            public const string SurveyService = "SurveyService";


        }

        public static OperationOutput GetOperationOutput(ServiceMessages header, params OutputDictionary[] outputs)
        {
            var result = (OperationOutput)Activator.CreateInstance(typeof(OperationOutput))!;

            if (outputs is not null)
            {
                result.Output = new Dictionary<string, object>();
                foreach (var output in outputs)
                    result.Output.Add(output.key, output.value);
            }
            result.Header = ApplicationOperation.OperationResult(header);


            return result;
        }
    }
    public record OutputDictionary(string key, object value);


}
