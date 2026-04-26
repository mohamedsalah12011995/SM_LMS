using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Permits.Dtos
{
    public class PermitAddAction
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        [JsonIgnore]
        public int? DeliverReferenceId { get; set; }
        public string deliverReferenceId { set { DeliverReferenceId = Accessor.Set(value); } get { return Accessor.Get(DeliverReferenceId); } }

        public int? PermitState { get; set; }
        public string Notes { get; set; }

        [JsonIgnore]
        public int? CurrentStep { get; set; }
        public string currentStep { set { CurrentStep = Accessor.Set(value); } get { return Accessor.Get(CurrentStep); } }


    }
}
