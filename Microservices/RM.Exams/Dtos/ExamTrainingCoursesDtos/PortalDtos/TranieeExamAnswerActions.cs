using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.PortalDtos
{

    public class TranieeExamAnswerActions
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        [JsonIgnore]
        public int? ExamId { get; set; }
        public string examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get<int?>(ExamId); } }

        [JsonIgnore]
        public int? TraineeExamId { get; set; }
        public string taineeExamId { set { TraineeExamId = Accessor.Set(value); } get { return Accessor.Get(TraineeExamId); } }

        [JsonIgnore]
        public int? CourseTypeId { get; set; }
        public string courseTypeId { set { CourseTypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(CourseTypeId); } }



        public string Note { get; set; }

        public List<ExamQuestionAnswer> ExamQuestionAnswers { get; set; } = new List<ExamQuestionAnswer>();

    }

}
