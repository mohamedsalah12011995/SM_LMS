using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Statistics.Dtos
{
    public class EntityItems
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public List<EntityItems> Items { get; set; } = new List<EntityItems>();
    }
}
