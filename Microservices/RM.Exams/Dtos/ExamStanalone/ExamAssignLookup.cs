using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamStanalone
{
    public class ExamAssignLookup
    {
        [JsonIgnore]
        public int? EntityId { get; set; }

        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }

        [JsonIgnore]
        public int? ReferenceId { get; set; }

        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }



    }
}
