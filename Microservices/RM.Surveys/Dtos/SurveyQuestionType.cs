using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Surveys.Dtos
{
    public class SurveyQuestionType : BaseDto<SurveyQuestionType, Models.SurveyQuestionType>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public bool? HasDataSource { get; set; }
        public string Icon { get; set; }




    }
}
