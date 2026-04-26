using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.CP_CourseAdvertisment
{
    public class CoursesAvailableOutput
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }


        [JsonIgnore]
        public int? _courseId { get; set; }
        public string courseId { set { _courseId = Accessor.Set(value); } get { return Accessor.Get(_courseId); } }


        [JsonIgnore]
        public int? _trainingCourseScheduleId { get; set; }
        public string trainingCourseScheduleId { set { _trainingCourseScheduleId = Accessor.Set(value); } get { return Accessor.Get(_trainingCourseScheduleId); } }

        [JsonIgnore]
        public int? _trainingCourseTypeId { get; set; }
        public string trainingCourseTypeId { set { _trainingCourseTypeId = Accessor.Set(value); } get { return Accessor.Get(_trainingCourseTypeId); } }


        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        public string TypeAr { get; set; }
        public string TypeEn { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.AdvertisementsCourses, CoursesAvailableOutput>()
                .Map(dest => dest.StartDate, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.StartDate.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.EndDate, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.EndDate.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest._trainingCourseScheduleId, src => src.TrainingCourseScheduleId)
                .Map(dest => dest.TitleAr, src => src.TrainingCourseSchedule != null ? $"{src.TrainingCourseSchedule.Course.TitleAr} - {(src.TrainingCourseSchedule.Course.Type == (int)ExamEnums.CourseType.Internal ? "تدريب داخلى" : "تدريب عام")}" : string.Empty)
                .Map(dest => dest.TitleEn, src => src.TrainingCourseSchedule != null ? $"{src.TrainingCourseSchedule.Course.TitleEn} - {(src.TrainingCourseSchedule.Course.Type == (int)ExamEnums.CourseType.Internal ? "Internal training" : "External training")}" : string.Empty)
                .Map(dest => dest._courseId, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.CourseId : null)
                .Map(dest => dest.TypeAr, src => src.TrainingCourseSchedule.Course.Type == (int)ExamEnums.CourseType.Internal ? "تدريب داخلى" : "تدريب عام")
                .Map(dest => dest.TypeEn, src => src.TrainingCourseSchedule.Course.Type == (int)ExamEnums.CourseType.Internal ? "Internal training" : "External training")
                .Map(dest => dest._trainingCourseTypeId, src => src.TrainingCourseSchedule.Course.Type)

                .Config;
        }


        public static TypeAdapterConfig SelectForAddConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.TrainingCourseSchedule, CoursesAvailableOutput>()
                .Map(dest => dest.StartDate, src => src.StartDate.ToString("yyyy-MM-dd"))
                .Map(dest => dest.EndDate, src => src.EndDate.ToString("yyyy-MM-dd"))
                .Map(dest => dest._trainingCourseScheduleId, src => src.Id)
                .Map(dest => dest._courseId, src => src.CourseId)
                .Map(dest => dest.TitleAr, src => $"{src.Course.TitleAr} - {(src.Course.Type == (int)ExamEnums.CourseType.Internal ? "تدريب داخلى" : "تدريب عام")}")
                .Map(dest => dest.TitleEn, src => $"{src.Course.TitleEn} - {(src.Course.Type == (int)ExamEnums.CourseType.Internal ? "Internal training" : "External training")}")
                .Map(dest => dest.TypeAr, src => src.Course.Type == (int)ExamEnums.CourseType.Internal ? "تدريب داخلى" : "تدريب عام")
                .Map(dest => dest.TypeEn, src => src.Course.Type == (int)ExamEnums.CourseType.Internal ? "Internal training" : "External training")
                .Map(dest => dest._trainingCourseTypeId, src => src.Course.Type)

                .Config;
        }

    }

}
