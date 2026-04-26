using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Users.Dtos
{
    public class MajorLookups
    {
        [JsonIgnore]
        internal int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
