using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Surveys.Dtos
{
    public class SurveyQuestionData
    {
        [JsonIgnore]
        public int? _id { get; set; }
        [JsonIgnore]
        public int? _createdBy { get; set; }

        [JsonIgnore]
        public int? _updateBy { get; set; }
        [JsonIgnore]
        public int? _deletedBy { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }

        public string CreatedBy { set { _createdBy = Accessor.Set(value); } get { return Accessor.Get(_createdBy); } }
        public string UpdatedBy { set { _updateBy = Accessor.Set(value); } get { return Accessor.Get(_updateBy); } }

        public string DeletedBy { set { _deletedBy = Accessor.Set(value); } get { return Accessor.Get(_deletedBy); } }


        [JsonIgnore]
        public int? _typeId { get; set; }
        public string TypeId { set { _typeId = Accessor.Set(value); } get { return Accessor.Get(_typeId); } }

        public string Type { get; set; }

        [JsonIgnore]
        public int? _surveyId { get; set; }
        public string SurveyId { set { _surveyId = Accessor.Set(value); } get { return Accessor.Get(_surveyId); } }


        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }

        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }


        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        public bool? Mandatory { get; set; }
        public bool? VerticalAnswersDirection { get; set; }
        public string GroupId { get; set; }

        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string StatusString { get; set; }
        public int? SubQuestionOrder { get; set; }
        public int? GroupOrder { get; set; }


        public List<SurveyQuestion> SubSurveyQuestions = new List<SurveyQuestion>();


    }
}
