using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.TrainingCourses
{
    public class TrainingCourseTypeDto
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }



    }
}
