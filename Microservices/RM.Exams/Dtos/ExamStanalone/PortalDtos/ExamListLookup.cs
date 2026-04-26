using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamStanalone.PortalDtos
{
    public class ExamListLookup
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

    }
}
