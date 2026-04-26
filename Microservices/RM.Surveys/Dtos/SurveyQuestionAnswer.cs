using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Surveys.Dtos
{
    public class SurveyQuestionAnswer
    {
        [JsonIgnore]
        public int? _id { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }
        [JsonIgnore]
        public int? _questionId { get; set; }
        public string QuestionId { set { _questionId = Accessor.Set(value); } get { return Accessor.Get(_questionId); } }
        [JsonIgnore]
        public int? _dataSourceId { get; set; }
        public string DataSourceId { set { _dataSourceId = Accessor.Set(value); } get { return Accessor.Get(_dataSourceId); } }
        [JsonIgnore]
        public int? _surveyAnswerActionId { get; set; }
        public string SurveyAnswerActionId { set { _surveyAnswerActionId = Accessor.Set(value); } get { return Accessor.Get(_surveyAnswerActionId); } }


        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }


        public string Text { get; set; }
        public int? Value { get; set; }

        public string QuestionType { get; set; }
        public string Notes { get; set; }


    }
}
