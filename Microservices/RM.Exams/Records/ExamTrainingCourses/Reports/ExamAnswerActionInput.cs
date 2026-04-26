using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Records.ExamTrainingCourses.Reports
{
    public record ExamAnswerActionInput
    {
        [JsonIgnore]
        public int? ExamCourseTranieeId { get; set; }
        public string examCourseTranieeId { set { ExamCourseTranieeId = Accessor.Set(value); } get { return Accessor.Get(ExamCourseTranieeId); } }


    }

}
