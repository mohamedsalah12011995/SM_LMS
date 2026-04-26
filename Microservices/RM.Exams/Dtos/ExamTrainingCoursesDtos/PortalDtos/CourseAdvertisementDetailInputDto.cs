using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.PortalDtos
{
    public class CourseAdvertisementPortalDetailInputDto
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        [JsonIgnore]
        public int? CourseTypeId { get; set; }
        public string courseTypeId { set { CourseTypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(CourseTypeId); } }


    }
}
