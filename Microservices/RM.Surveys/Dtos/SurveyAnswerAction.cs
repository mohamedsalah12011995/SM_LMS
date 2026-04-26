using RM.Core.Helpers;
using System.Text.Json.Serialization;


namespace RM.Surveys.Dtos
{
    public class SurveyAnswerAction
    {
        [JsonIgnore]
        public int? _id { get; set; }
        [JsonIgnore]
        public int? _createdBy { get; set; }

        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }

        public string CreatedBy { set { _createdBy = Accessor.Set(value); } get { return Accessor.Get(_createdBy); } }
        [JsonIgnore]
        public int? _surveyId { get; set; }
        public string SurveyId { set { _surveyId = Accessor.Set(value); } get { return Accessor.Get(_surveyId); } }

        public DateTime? CreatedDate { get; set; }

        public bool? UseCapcha { get; set; }


        public string Capcha { get; set; }
        public List<SurveyQuestionAnswer> SurveyQuestionAnswers { get; set; } = new List<SurveyQuestionAnswer>();

    }
}
