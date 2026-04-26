using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.AssignTrainee
{
    public class AssignInternalTraineesInputDto : BaseDto<AssignInternalTraineesInputDto, Models.InternalCourseTrainees>
    {

        [JsonIgnore]
        public int? CourseId { get; set; }
        public string courseId { set { CourseId = Accessor.Set(value); } get { return Accessor.Get(CourseId); } }

        [JsonIgnore]
        public int? TrainingCourseScheduleId { get; set; }
        public string trainingCourseScheduleId { set { TrainingCourseScheduleId = Accessor.Set(value); } get { return Accessor.Get(TrainingCourseScheduleId); } }

        public List<InternTrainees> Trainees { get; set; }

        public TypeAdapterConfig AddConfig(int userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<AssignInternalTraineesInputDto, Models.InternalCourseTrainees>().IgnoreNullValues(true)
                .Map(dest => dest.Code, src => $"{currentDate.ToString("yyMMddhhmm")}{Strings.RandomDigits(4).ToString()}")
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => currentDate)
                .Map(dest => dest.IsDeleted, src => false)


                .Config;
        }


        public TypeAdapterConfig UpdateConfig(int? userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<AssignInternalTraineesInputDto, Models.InternalCourseTrainees>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => currentDate)


                .Config;
        }



    }


    public class InternTrainees
    {
        [JsonIgnore]
        public int? TraineeId { get; set; }
        public string traineeId { set { TraineeId = Accessor.Set(value); } get { return Accessor.Get(TraineeId); } }

        public string Name { get; set; }
        public string IdCardNumber { get; set; }
        public string EmployeeID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

    }

}
