using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.AssignTrainee
{
    public class SendEmailToTrainees
    {
        [JsonIgnore]
        public int? CourseId { get; set; }
        public string courseId { set { CourseId = Accessor.Set(value); } get { return Accessor.Get(CourseId); } }

        [JsonIgnore]
        public int? TrainingCourseScheduleId { get; set; }
        public string trainingCourseScheduleId { set { TrainingCourseScheduleId = Accessor.Set(value); } get { return Accessor.Get(TrainingCourseScheduleId); } }

        public List<TraineeIds> Trainees { get; set; }

    }

    public class TraineeIds
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
    }





}
