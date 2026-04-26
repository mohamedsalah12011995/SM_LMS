using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Records.ExamTrainingCourses.Reports
{
    public record ExamResultByYear
    {
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }

        [JsonIgnore]
        public int? DepartmentReferenceId { get; set; }
        public string departmentReferenceId { set { DepartmentReferenceId = Accessor.Set(value); } get { return Accessor.Get(DepartmentReferenceId); } }


        [JsonIgnore]
        public int? TypeId { get; set; }
        public string typeId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get(TypeId); } }

        public int Year { get; set; }

    }

    public record CourseScheduleReport(string Code, int Id, int CourseId, DateTime StartDate, DateTime EndDate, string CourseTitleAr, string CourseTitleEn, int CourseType, string TypeAr = null, string TypeEn = null);

    public record ExamTotalTraniee(int? TrainingCourseScheduleId, int TotalTrainee, int TotalPassTraniee, double SuccessRate);
}
