using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Permits.Dtos
{
    public class QueryPersonPermitRequests
    {
        public string IdentityNumber { get; set; }

        public string Code { get; set; }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }

        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }


        public string ItemUrl { get; set; }

    }
}
