
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.PortalDtos
{
    public class ExternalCourseTranieeInputDto : BaseDto<ExternalCourseTranieeInputDto, Models.ExternalCourseTraniees>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        [JsonIgnore]
        public int? TrainingCourseScheduleId { get; set; }
        public string trainingCourseScheduleId { set { TrainingCourseScheduleId = Accessor.Set(value); } get { return Accessor.Get(TrainingCourseScheduleId); } }


        [JsonIgnore]
        public int? GradeType { get; set; }
        public string gradeType { set { GradeType = Accessor.Set(value); } get { return Accessor.Get(GradeType); } }

        public string FullName { get; set; }
        public string IdCardNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? GradeYear { get; set; }
        public string GradeTitle { get; set; }

        public TypeAdapterConfig AddConfig(int userId, int courseId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<ExternalCourseTranieeInputDto, Models.ExternalCourseTraniees>().IgnoreNullValues(true)
                .Map(dest => dest.Code, src => $"{currentDate.ToString("yyMMddhhmm")}{Strings.RandomDigits(4).ToString()}")
                .Map(dest => dest.CourseId, src => courseId)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => currentDate)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.Status, src => (int)ExamEnums.TraineeStatus.Waiting)

                .Config;
        }


        public TypeAdapterConfig UpdateConfig(int? userId, int courseId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<ExternalCourseTranieeInputDto, Models.ExternalCourseTraniees>().IgnoreNullValues(true)
                .Map(dest => dest.CourseId, src => courseId)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => currentDate)
                .Map(dest => dest.Status, src => (int)ExamEnums.TraineeStatus.Waiting)


                .Config;
        }

    }
}
