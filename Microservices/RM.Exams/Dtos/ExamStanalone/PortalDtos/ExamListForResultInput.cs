using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamStanalone.PortalDtos
{
    public class ExamListForResultInput
    {
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]

        public int? EntityId { get; set; }

        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
    }
}
