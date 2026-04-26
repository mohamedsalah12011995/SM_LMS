using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Records.ExamTrainingCourses
{
    public record PaginateInternalUsers
    {

        [JsonIgnore]
        public int? TrainingCourseScheduleId { get; set; }

        public string trainingCourseScheduleId { set { TrainingCourseScheduleId = Accessor.Set(value); } get { return Accessor.Get<int?>(TrainingCourseScheduleId); } }

        [JsonIgnore]
        public int? DepartmentReferenceId { get; set; }

        public string departmentReferenceId { set { DepartmentReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(DepartmentReferenceId); } }
        public ApplicationOperation.Pagination Pagination { get; set; }

    }

}
