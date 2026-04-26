using RM.Core.Helpers;
using System.Text.Json.Serialization;


namespace RM.Surveys.Dtos
{
    public class GroupQuestion
    {

        public string GroupId { get; set; }
        [JsonIgnore]
        public int? _typeId { get; set; }
        public string TypeId { set { _typeId = Accessor.Set(value); } get { return Accessor.Get(_typeId); } }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }

        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }

        public List<SurveyQuestion> SurveyQuestion { get; set; }
        public List<SurveyDataSource> GroupDataSource { get; set; }




    }
}
