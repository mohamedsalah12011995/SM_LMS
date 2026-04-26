using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.CP_Reports
{
    public class TranieeByScheduleId
    {
        [JsonIgnore]
        public int? Id { get; set; }// id table InternalCourseTraniee or External
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        [JsonIgnore]
        public int? TrainingCourseScheduleId { get; set; }
        public string trainingCourseScheduleId { set { TrainingCourseScheduleId = Accessor.Set(value); } get { return Accessor.Get(TrainingCourseScheduleId); } }

        public string TranieeName { get; set; }
        public string Code { get; set; }
        public bool? IsAttendedExam { get; set; }

    }

    public class ExamTranieeByScheduleId
    {
        [JsonIgnore]
        public int? ExamCourseTranieeId { get; set; }// id table InternalExamTraniee or External
        public string examCourseTranieeId { set { ExamCourseTranieeId = Accessor.Set(value); } get { return Accessor.Get(ExamCourseTranieeId); } }

        [JsonIgnore]
        public int? ScuduleCourseTranieeId { get; set; }// id table InternalExamTraniee or External
        public string scuduleCourseTranieeId { set { ScuduleCourseTranieeId = Accessor.Set(value); } get { return Accessor.Get(ScuduleCourseTranieeId); } }

        public string ExamDate { get; set; }
        public string ExamTime { get; set; }
        public double Result { get; set; }
        public bool? IsSuccess { get; set; }
        public double SuccessRate { get; set; }

    }
}
