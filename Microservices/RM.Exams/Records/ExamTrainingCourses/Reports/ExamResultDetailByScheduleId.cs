using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Records.ExamTrainingCourses.Reports
{
    public record ExamResultDetailByScheduleId
    {
        [JsonIgnore]
        public int? TrainingCourseScheduleId { get; set; }
        public string trainingCourseScheduleId { set { TrainingCourseScheduleId = Accessor.Set(value); } get { return Accessor.Get(TrainingCourseScheduleId); } }

        public ApplicationOperation.Pagination Pagination { get; set; }

    }




}
