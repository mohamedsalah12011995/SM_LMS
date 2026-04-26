using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.AssignToExam
{
    public class ExternalTraineeForExamOutput : BaseDto<ExternalTraineeForExamOutput, Models.ExternalCourseTraniees>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public string Code { get; set; }
        public string FullName { get; set; }
        public string IdCardNumber { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }

        public string GenderAr { get; set; }
        public string GenderEn { get; set; }

        public string GradeTypeAr { get; set; }
        public string GradeTypeEn { get; set; }
        public string GradeTitle { get; set; }
        public int? GradeYear { get; set; }
        public int Status { get; set; }
        public bool IsSelected { get; set; }


    }


}
