using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.CP_CourseAdvertisment
{
    public class CourseAdvertisementOutputDetailDto : BaseDto<CourseAdvertisementOutputDetailDto, Models.CourseAdvertisement>
    {
        public CourseAdvertisementOutputDetailDto()
        {
            AdvertisementCourses = new List<CoursesAvailableOutput>();
        }
        [JsonIgnore]
        public int? Id { get; set; }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }

        public List<CoursesAvailableOutput> AdvertisementCourses { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.CourseAdvertisement, CourseAdvertisementOutputDetailDto>()
                .Map(dest => dest.AdvertisementCourses, src => src.AdvertisementsCourses != null ? src.AdvertisementsCourses.Adapt<List<CoursesAvailableOutput>>(CoursesAvailableOutput.SelectConfig()) : new List<CoursesAvailableOutput>())

                .Config;
        }



    }

}
