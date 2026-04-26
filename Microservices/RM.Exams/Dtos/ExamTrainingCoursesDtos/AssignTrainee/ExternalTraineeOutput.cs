using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.AssignTrainee
{
    public class ExternalTraineeOutput : BaseDto<ExternalTraineeOutput, Models.ExternalCourseTraniees>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

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
        public bool IsSelected { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.ExternalCourseTraniees, ExternalTraineeOutput>()
                .Map(dest => dest.GenderAr, src => src.Gender == (int)Enums.Gender.Male ? "ذكر" : "انثى")
                .Map(dest => dest.GenderEn, src => src.Gender == (int)Enums.Gender.Male ? "Male" : "Female")

                .Map(dest => dest.GradeTypeAr, src => src.Grade != null ? src.Grade.NameAr : string.Empty)
                .Map(dest => dest.GradeTypeEn, src => src.Grade != null ? src.Grade.NameEn : string.Empty)
                .Map(dest => dest.IsSelected, src => src.Status == (int)ExamEnums.TraineeStatus.Enrolled ? true : false)

                  .Config;
        }
    }


}
