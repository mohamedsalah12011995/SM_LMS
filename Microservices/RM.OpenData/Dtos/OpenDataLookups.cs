

using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.OpenData.Dtos
{
    public class OpenDataLookups
    {

        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        [JsonIgnore]
        public int? MapId { get; set; }
        public string mapId { set { MapId = Accessor.Set(value); } get { return Accessor.Get<int?>(MapId); } }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int TypeId { get; set; }

        public List<OpenDataLookups> SubTypes { get; set; } = new List<OpenDataLookups>();

    }
}
