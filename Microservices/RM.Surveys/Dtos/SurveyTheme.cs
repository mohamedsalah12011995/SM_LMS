using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Surveys.Dtos
{
    public class SurveyTheme : BaseDto<SurveyTheme, Models.SurveyTheme>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

    }
}
