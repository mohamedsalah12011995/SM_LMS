using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.TrainingCourseSchedule
{
    public class TrainingCourseScheduleInputDto : BaseDto<TrainingCourseScheduleInputDto, Models.TrainingCourseSchedule>
    {
        [JsonIgnore]
        public int? Id { get; set; }


        [JsonIgnore]
        public int? CourseId { get; set; }

        [JsonIgnore]
        public int? ReferenceId { get; set; }

        [JsonIgnore]
        public int? DepartmentReferenceId { get; set; }

        [JsonIgnore]
        public int? ExamId { get; set; }

        [JsonIgnore]
        public int? CertificateId { get; set; }
        [JsonIgnore]
        public int? CertificateThemeId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string courseId { set { CourseId = Accessor.Set(value); } get { return Accessor.Get(CourseId); } }
        public string certificateId { set { CertificateId = Accessor.Set(value); } get { return Accessor.Get(CertificateId); } }
        public string certificateThemeId { set { CertificateThemeId = Accessor.Set(value); } get { return Accessor.Get(CertificateThemeId); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string departmentReferenceId { set { DepartmentReferenceId = Accessor.Set(value); } get { return Accessor.Get(DepartmentReferenceId); } }
        public string examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get(ExamId); } }


        public TypeAdapterConfig AddConfig(int userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<TrainingCourseScheduleInputDto, Models.TrainingCourseSchedule>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => currentDate)
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.IsClosed, src => false)
                .Map(dest => dest.IsDeleted, src => false)

                .Config;
        }


        public TypeAdapterConfig UpdateConfig(int? userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<TrainingCourseScheduleInputDto, Models.TrainingCourseSchedule>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => currentDate)
                .Config;
        }



    }
}
