using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.CP_Reports
{
    public class ExamResultDetailOutputDto
    {
        [JsonIgnore]
        public int? Id { get; set; } // id table InternalCourseTraniee or External
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        [JsonIgnore]
        public int? ExamCourseTranieeId { get; set; }// id table InternalExamTraniee or External
        public string examCourseTranieeId { set { ExamCourseTranieeId = Accessor.Set(value); } get { return Accessor.Get(ExamCourseTranieeId); } }

        public string Code { get; set; }
        public string TranieeName { get; set; }
        public bool? IsAttendedExam { get; set; }
        public string AttendedExamAr { get; set; }
        public string AttendedExamEn { get; set; }
        public double Result { get; set; }
        public string StatusAr { get; set; }
        public string StatusEn { get; set; }
        public string SuccessRate { get; set; }

    }


}
