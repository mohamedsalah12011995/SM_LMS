using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.CP_CourseAdvertisment
{
    public class CourseAdvertisementOutputListDto : BaseDto<CourseAdvertisementOutputListDto, Models.CourseAdvertisement>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }


        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string StatusStringAr { get; set; }
        public string StatusStringEn { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }



        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.CourseAdvertisement, CourseAdvertisementOutputListDto>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.StatusStringAr, src => src.IsActive == true ? "فعالة" : "غير مفعلة")
                .Map(dest => dest.StatusStringEn, src => src.IsActive == true ? "Active" : "Inactive")
                .Map(dest => dest.FromDate, src => src.FromDate.ToString("yyyy-MM-dd"))
                .Map(dest => dest.ToDate, src => src.ToDate.ToString("yyyy-MM-dd"))
                .Config;
        }


    }
}
