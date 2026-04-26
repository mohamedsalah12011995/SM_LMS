using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Records.ExamTrainingCourses
{
    public record ActivationCorsesInputDto
    {
        [JsonIgnore]
        public int? ReferenceId { get; set; }

        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }


        [JsonIgnore]
        public int? DepartmentReferenceId { get; set; }

        public string departmentReferenceId { set { DepartmentReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(DepartmentReferenceId); } }

        [JsonIgnore]
        public int? Type { get; set; }

        public string type { set { Type = Accessor.Set(value); } get { return Accessor.Get<int?>(Type); } }

    }

}
