using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Permits.Dtos
{
    public class FlowStepperProject
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        [JsonIgnore]
        public int? ProjectId { get; set; }
        public string projectId { set { ProjectId = Accessor.Set(value); } get { return Accessor.Get<int?>(ProjectId); } }
        [JsonIgnore]
        public int? StepId { get; set; }
        public string stepId { set { StepId = Accessor.Set(value); } get { return Accessor.Get<int?>(StepId); } }

        public int? OrderStep { get; set; }

    }
}
