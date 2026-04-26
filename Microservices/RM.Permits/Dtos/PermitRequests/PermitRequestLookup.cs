using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Permits.Dtos
{
    public class PermitRequestLookup
    {
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? LeadersReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string leadersReferenceId { set { LeadersReferenceId = Accessor.Set(value); } get { return Accessor.Get(LeadersReferenceId); } }
    }
}
