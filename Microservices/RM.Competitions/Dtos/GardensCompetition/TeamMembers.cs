
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Competitions.Dtos
{
    public class TeamMembers
    {
        [JsonIgnore]
        public int? _id { get; set; }
        [JsonIgnore]
        public int? _competitorId { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get<int?>(_id); } }
        public string CompetitorId { set { _competitorId = Accessor.Set(value); } get { return Accessor.Get<int?>(_competitorId); } }
        public string FullName { get; set; }
        public string IdCard { get; set; }
        public string Phone { get; set; }
    }
}
