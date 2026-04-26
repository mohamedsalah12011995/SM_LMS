using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.PortalDtos
{
    public class CourseAdvertisementListOutputDto
    {
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

        public static TypeAdapterConfig SelectConfig(DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.CourseAdvertisement, CourseAdvertisementListOutputDto>()

                .Map(dest => dest.FromDate, src => src.FromDate.ToString("yyyy-MM-dd"))
                .Map(dest => dest.ToDate, src => src.ToDate.ToString("yyyy-MM-dd"))
                .Map(dest => dest.RemainDayes, src => src.ToDate.Date.Subtract(currentDate.Date).TotalDays)
                .Config;
        }

    }


}
