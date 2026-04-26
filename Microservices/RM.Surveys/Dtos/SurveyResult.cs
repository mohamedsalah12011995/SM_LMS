using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;


namespace RM.Surveys.Dtos
{
    public class SurveyResult
    {
        [JsonIgnore]
        public int? _surveyId { get; set; }
        public string SurveyId { set { _surveyId = Accessor.Set(value); } get { return Accessor.Get(_surveyId); } }

        public string SurveyTitle { get; set; }
        public string SurveyTitleEn { get; set; }


        [JsonIgnore]
        public int? _questionId { get; set; }
        public string QuestionId { set { _questionId = Accessor.Set(value); } get { return Accessor.Get(_questionId); } }

        [JsonIgnore]
        public int? _questionTypeId { get; set; }
        public string QuestionTypeId { set { _questionTypeId = Accessor.Set(value); } get { return Accessor.Get(_questionTypeId); } }

        public string Question { get; set; }
        public string QuestionEn { get; set; }


        public int? Count { get; set; }
        public int? Sum { get; set; }
        public int? Rate { get; set; }

        public double Avg { get; set; }

        public string GroupId { get; set; }
        public string AnswerText { get; set; }
        public int? AnswerValue { get; set; }

        //public string QuestionRecommendationAr { get; set; }
        //public string QuestionRecommendationEn { get; set; }

        [JsonIgnore]
        public int? _dataSourceId { get; set; }
        public string DataSourceId { set { _dataSourceId = Accessor.Set(value); } get { return Accessor.Get(_dataSourceId); } }
        public string Choice { get; set; }
        public string ChoiceEn { get; set; }

        public List<string> DropDownIds { get; set; } = new List<string>();
        public List<string> Emails { get; set; } = new List<string>();

        public string Subject { get; set; }
        public string Body { get; set; }
        public string FileName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Survey, SurveyResult>()
                .Map(dest => dest._surveyId, src => src.Id)
                .Map(dest => dest.SurveyTitle, src => src.TitleAr)
                .Map(dest => dest.SurveyTitleEn, src => src.TitleEn)
                .Map(dest => dest.Subject, src => src.TitleEn)
                .Map(dest => dest.FileName, src => src.TitleEn.Replace(" ","_"))
                .Config;
        }

        public static TypeAdapterConfig SelectWithSettingsConfig(int cronTypeId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Survey, SurveyResult>()
                .Map(dest => dest._surveyId, src => src.Id)
                .Map(dest => dest.SurveyTitle, src => src.TitleAr)
                .Map(dest => dest.SurveyTitleEn, src => src.TitleEn)
                .Map(dest => dest.Subject, src => src.TitleEn)
                .Map(dest => dest.FileName, src => src.TitleEn.Replace(" ", "_"))
                .Map(dest => dest.Emails, src => src.CronSettings != null ? src.CronSettings.Where(x=>x.CronTypeId == cronTypeId && x.IsActive == true).FirstOrDefault() != null ? Strings.ConvertStringToList(src.CronSettings.Where(x => x.CronTypeId == cronTypeId && x.IsActive == true).FirstOrDefault().Emails, "$") : new List<string>() : new List<string>())
                .Config;
        }
    }


}
