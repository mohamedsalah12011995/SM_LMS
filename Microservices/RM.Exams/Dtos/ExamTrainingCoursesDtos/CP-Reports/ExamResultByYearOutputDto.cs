using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.CP_Reports
{
    public class ExamResultByYearOutputDto
    {
        [JsonIgnore]
        public int? CourseId { get; set; }
        public string courseId { set { CourseId = Accessor.Set(value); } get { return Accessor.Get(CourseId); } }

        public string Code { get; set; }
        public string CourseTitleAr { get; set; }
        public string CourseTitleEn { get; set; }

        public List<TrainingCourseScheduleReport> TrainingCourseSchedule { get; set; }

    }
    public class TrainingCourseScheduleReport
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int TotalTraniee { get; set; }
        public int NumberOfExamApplicants { get; set; }
        public int TotalPassTraniee { get; set; }
        public string SuccessRate { get; set; }

    }

}
