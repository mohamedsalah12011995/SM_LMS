using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.TrainingCourseSchedule
{
    public class TrainingCourseScheduleOutputDto : BaseDto<TrainingCourseScheduleInputDto, Models.TrainingCourseSchedule>
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
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string departmentReferenceId { set { DepartmentReferenceId = Accessor.Set(value); } get { return Accessor.Get(DepartmentReferenceId); } }
        public string examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get(ExamId); } }

        public string certificateId { set { CertificateId = Accessor.Set(value); } get { return Accessor.Get(CertificateId); } }
        public string certificateThemeId { set { CertificateThemeId = Accessor.Set(value); } get { return Accessor.Get(CertificateThemeId); } }

        public bool IsEditAvailable { get; set; }

        public string CertificateTitleAr { get; set; }
        public string CertificateTitleEn { get; set; }
        public static TypeAdapterConfig SelectConfig(DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.TrainingCourseSchedule, TrainingCourseScheduleOutputDto>()
                .Map(dest => dest.CertificateTitleAr, src => src.Certificate != null ? src.Certificate.TitleAr : string.Empty)
                .Map(dest => dest.CertificateTitleEn, src => src.Certificate != null ? src.Certificate.TitleEn : string.Empty)
                .Map(dest => dest.IsEditAvailable, src => src.EndDate.Date > currentDate.Date)

                  .Config;
        }


    }
}
