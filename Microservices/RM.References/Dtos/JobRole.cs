using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.References.Dtos
{
    public class JobRole
    {
        [JsonIgnore]
        public int? _id { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
