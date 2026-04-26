using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Records.ExamTrainingCourses
{
    public record CorsesScheduleByCourseId
    {
        [JsonIgnore]
        public int? CourseId { get; set; }

        public string courseId { set { CourseId = Accessor.Set(value); } get { return Accessor.Get<int?>(CourseId); } }

        [JsonIgnore]
        public int? ReferenceId { get; set; }

        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }



        [JsonIgnore]
        public int? DepartmentReferenceId { get; set; }

        public string departmentReferenceId { set { DepartmentReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(DepartmentReferenceId); } }

        //   public ApplicationOperation.Pagination Pagination { get; set; }

    }

}
