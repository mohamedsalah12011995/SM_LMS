using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Permits.Dtos
{
    public class PrintPermitDto
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        [JsonIgnore]
        public int? DeliverReferenceId { get; set; }
        public string deliverReferenceId { set { DeliverReferenceId = Accessor.Set(value); } get { return Accessor.Get(DeliverReferenceId); } }
        [JsonIgnore]
        public int? LastPrintRequestId { get; set; }
        public string lastPrintRequestId { set { LastPrintRequestId = Accessor.Set(value); } get { return Accessor.Get(LastPrintRequestId); } }

    }
}
