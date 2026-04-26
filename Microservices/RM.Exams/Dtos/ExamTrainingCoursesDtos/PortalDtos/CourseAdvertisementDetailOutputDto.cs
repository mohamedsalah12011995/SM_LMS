using Mapster;
using RM.Core.Helpers;
using RM.Exams.ExamEnums;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.PortalDtos
{
    public class CourseAdvertisementDetailOutputDto
    {
        public CourseAdvertisementDetailOutputDto()
        {
            Courses = new List<AdvertisementsCoursesDetailOutput>();
        }
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string RemainDayes { get; set; }
        public List<AdvertisementsCoursesDetailOutput> Courses { get; set; }

        public static TypeAdapterConfig SelectConfig(DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.CourseAdvertisement, CourseAdvertisementDetailOutputDto>()

                .Map(dest => dest.FromDate, src => src.FromDate.ToString("yyyy-MM-dd"))
                .Map(dest => dest.ToDate, src => src.ToDate.ToString("yyyy-MM-dd"))
                .Map(dest => dest.RemainDayes, src => src.ToDate.Date.Subtract(currentDate.Date).TotalDays)
                .Map(dest => dest.Courses, src => src.AdvertisementsCourses != null ? src.AdvertisementsCourses.Adapt<List<AdvertisementsCoursesDetailOutput>>(AdvertisementsCoursesDetailOutput.SelectConfig()) : new List<AdvertisementsCoursesDetailOutput>())


                .Config;
        }




    }

    public class AdvertisementsCoursesDetailOutput
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        [JsonIgnore]
        public int? _trainingCourseScheduleId { get; set; }
        public string trainingCourseScheduleId { set { _trainingCourseScheduleId = Accessor.Set(value); } get { return Accessor.Get<int?>(_trainingCourseScheduleId); } }

        [JsonIgnore]
        public int? _courseTypeId { get; set; }
        public string courseTypeId { set { _courseTypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(_courseTypeId); } }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string CourseTitleAr { get; set; }
        public string CourseTitleEn { get; set; }

        public string CourseDescriptionAr { get; set; }
        public string CourseDescriptionEn { get; set; }
        public string CourseTypeAr { get; set; }
        public string CourseTypeEn { get; set; }
        public bool LoginRequired { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.AdvertisementsCourses, AdvertisementsCoursesDetailOutput>()

                .Map(dest => dest.StartDate, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.StartDate.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.EndDate, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.EndDate.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest._courseTypeId, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.Course.Type : 0)
                .Map(dest => dest._trainingCourseScheduleId, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.Id : 0)

                .Map(dest => dest.LoginRequired, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.Course.Type == (int)CourseType.Internal ? true : false : false)

                .Map(dest => dest.CourseTypeAr, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.Course.Type == (int)ExamEnums.CourseType.Internal
                ? "تدريب داخلى للعاملين" : "تدريب عام" : string.Empty)

                .Map(dest => dest.CourseTypeEn, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.Course.Type == (int)ExamEnums.CourseType.Internal
                ? "Internal Training for Employees" : "General Training" : string.Empty)

                .Map(dest => dest.CourseTitleAr, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.Course.TitleAr : string.Empty)
                .Map(dest => dest.CourseTitleEn, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.Course.TitleEn : string.Empty)

                 .Map(dest => dest.CourseDescriptionAr, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.Course.DescriptionAr : string.Empty)
                 .Map(dest => dest.CourseDescriptionEn, src => src.TrainingCourseSchedule != null ? src.TrainingCourseSchedule.Course.DescriptionEn : string.Empty)

                .Config;
        }


    }

    public class CoursesDetailOutput
    {
        [JsonIgnore]
        public int? _id { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get<int?>(_id); } }
        [JsonIgnore]
        public int? _trainingCourseScheduleId { get; set; }
        public string trainingCourseScheduleId { set { _trainingCourseScheduleId = Accessor.Set(value); } get { return Accessor.Get<int?>(_trainingCourseScheduleId); } }

        [JsonIgnore]
        public int? _courseTypeId { get; set; }
        public string courseTypeId { set { _courseTypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(_courseTypeId); } }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string CourseTitleAr { get; set; }
        public string CourseTitleEn { get; set; }

        public string CourseDescriptionAr { get; set; }
        public string CourseDescriptionEn { get; set; }
        public string CourseTypeAr { get; set; }
        public string CourseTypeEn { get; set; }
        public bool LoginRequired { get; set; }



        public static TypeAdapterConfig SelectForCourseDetailConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.TrainingCourseSchedule, CoursesDetailOutput>()

                .Map(dest => dest._id, src => src.Course.Id)
                .Map(dest => dest.CourseTitleAr, src => src.Course.TitleAr)
                .Map(dest => dest.CourseTitleEn, src => src.Course.TitleEn)
                .Map(dest => dest.CourseDescriptionAr, src => src.Course.DescriptionAr)
                .Map(dest => dest.CourseDescriptionEn, src => src.Course.DescriptionEn)
                 .Map(dest => dest._courseTypeId, src => src.Course.Type)
                .Map(dest => dest._trainingCourseScheduleId, src => src.Id)

                .Map(dest => dest.LoginRequired, src => src.Course.Type == (int)CourseType.Internal ? true : false)

                .Map(dest => dest.CourseTypeAr, src => src.Course.Type == (int)ExamEnums.CourseType.Internal
                ? "تدريب داخلى للعاملين" : "تدريب عام")

                .Map(dest => dest.CourseTypeEn, src => src.Course.Type == (int)ExamEnums.CourseType.Internal
                ? "Internal Training for Employees" : "General Training")


                .Config;
        }

    }
}
