using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Competitions.Dtos
{
    public class CompetitorsType
    {
        [JsonIgnore]
        internal int? _id { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get<int?>(_id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
