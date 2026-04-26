using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Statistics.Dtos
{
    public class EntitiesLatestUpdate
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public DateTime? LastUpdate { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
