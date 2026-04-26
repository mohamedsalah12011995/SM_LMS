using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Records.ExamTrainingCourses
{
    public record LookupCourseScheduleForAdd
    {
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }

        [JsonIgnore]
        public int? ExamEntityId { get; set; }
        public string examEntityId { set { ExamEntityId = Accessor.Set(value); } get { return Accessor.Get(ExamEntityId); } }

    }
}
