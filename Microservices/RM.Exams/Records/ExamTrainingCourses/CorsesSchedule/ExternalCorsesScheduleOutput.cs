using RM.Core.Helpers;
using RM.Exams.Dtos.ExamTrainingCourses.AssignTrainee;
using System.Text.Json.Serialization;

namespace RM.Exams.Records.ExamTrainingCourses
{
    public record ExternalCorsesScheduleOutput
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        public string StartDateString { get; set; }
        public string EndDateString { get; set; }

        public List<ExternalTraineeOutput> Trainees { get; set; }

    }
}
