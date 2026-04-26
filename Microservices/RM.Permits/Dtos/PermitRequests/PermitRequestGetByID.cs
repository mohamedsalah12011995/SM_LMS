using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Permits.Dtos
{
    public class PermitRequestGetByID
    {
        [JsonIgnore]
        public int? _id { get; set; }
        public string Id { set { _id = Accessor.Set(value); } get { return Accessor.Get(Id); } }


    }
}
