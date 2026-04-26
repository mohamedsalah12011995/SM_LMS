using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.PortalDtos
{
    public class QueryInternalTraineeOutputDto
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        [JsonIgnore]
        public int? _courseId { get; set; }
        public string courseId { set { _courseId = Accessor.Set(value); } get { return Accessor.Get(_courseId); } }

        [JsonIgnore]
        public int? _courseTypeId { get; set; }
        public string courseTypeId { set { _courseTypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(_courseTypeId); } }

        public string CourseNameAr { get; set; }
        public string CourseNameEn { get; set; }

        public string TraineeName { get; set; }
        public string IdCardNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string EmployeeId { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StatusAr { get; set; }
        public string StatusEn { get; set; }

        [JsonIgnore]
        public int? TranieeExamId { get; set; }
        public string tranieeExamId { set { TranieeExamId = Accessor.Set(value); } get { return Accessor.Get<int?>(TranieeExamId); } }

        public bool IsExamAvailable { get; set; }

        public string ExamTitleAr { get; set; }
        public string ExamTitleEn { get; set; }
        public string ExamDateStringAr { get; set; }
        public string ExamDateStringEn { get; set; }
        public string ExamResultStringAr { get; set; }
        public string ExamResultStringEn { get; set; }
        public bool? IsLoginRequired { get; set; }
        public bool? IsSuccess { get; set; }

        public TraineeCertificate TraineeCertificate { get; set; }



        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.InternalCourseTrainees, QueryInternalTraineeOutputDto>()
                .Map(dest => dest._courseId, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.CourseId : null)
                .Map(dest => dest.CourseNameAr, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.Course.TitleAr : string.Empty)
                .Map(dest => dest.CourseNameEn, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.Course.TitleEn : string.Empty)
                .Map(dest => dest.IsLoginRequired, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.Course.LoginRequired : false)

                .Map(dest => dest._courseTypeId, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.Course.Type : 0)
                .Map(dest => dest.StartDate, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.StartDate.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.EndDate, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.EndDate.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.StatusAr, src => "متدرب")
                .Map(dest => dest.StatusEn, src => "Enrolled")

                  .Config;
        }
    }


}
