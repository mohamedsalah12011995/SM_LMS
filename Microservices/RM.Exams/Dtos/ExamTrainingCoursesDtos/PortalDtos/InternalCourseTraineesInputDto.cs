using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.PortalDtos
{
    public class InternalCourseTraineesInputDto : BaseDto<InternalCourseTraineesInputDto, Models.InternalCourseTrainees>
    {

        [JsonIgnore]
        public int? TrainingCourseScheduleId { get; set; }
        public string trainingCourseScheduleId { set { TrainingCourseScheduleId = Accessor.Set(value); } get { return Accessor.Get(TrainingCourseScheduleId); } }

        public string IdCardNumber { get; set; }

        public TypeAdapterConfig AddConfig(int userId, int courseId, Models.User userData, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<InternalCourseTraineesInputDto, Models.InternalCourseTrainees>().IgnoreNullValues(true)
                .Map(dest => dest.Code, src => $"{currentDate.ToString("yyMMddhhmm")}{Strings.RandomDigits(4).ToString()}")
                .Map(dest => dest.CourseId, src => courseId)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => currentDate)
                .Map(dest => dest.TraineeId, src => userData.Id)
                .Map(dest => dest.EmployeeID, src => userData.EmployeeId)
                .Map(dest => dest.Email, src => userData.Email)
                .Map(dest => dest.TraineeName, src => userData.Name)
                .Map(dest => dest.Phone, src => userData.Phone)
                .Map(dest => dest.IsDeleted, src => false)

                .Config;
        }


        public TypeAdapterConfig UpdateConfig(int? userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<ExternalCourseTranieeInputDto, Models.ExternalCourseTraniees>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => currentDate)
                .Map(dest => dest.Status, src => (int)ExamEnums.TraineeStatus.Waiting)


                .Config;
        }


    }

}
