using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.PortalDtos
{
    public class GetTraineeExamDto
    {
        [JsonIgnore]
        public int? TraineeExamId { get; set; }
        public string taineeExamId { set { TraineeExamId = Accessor.Set(value); } get { return Accessor.Get(TraineeExamId); } }

        [JsonIgnore]
        public int? CourseTypeId { get; set; }
        public string courseTypeId { set { CourseTypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(CourseTypeId); } }

    }
}
