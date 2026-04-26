using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;


namespace RM.Surveys.Dtos
{
    public class SurveyDataSource : BaseDto<SurveyDataSource, Models.SurveyDataSource>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public string createdById { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }
        public string deletedById { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get(DeletedBy); } }
        public string updatedById { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get(UpdatedBy); } }

        [JsonIgnore]
        public int? QuestionId { get; set; }
        public string questionID { set { QuestionId = Accessor.Set(value); } get { return Accessor.Get(QuestionId); } }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string StatusString { get; set; }

        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string GroupId { get; set; }
        public string Image { get; set; }
        public string ImageBase64 { get; set; }
        public int Rate { get; set; }


        public static TypeAdapterConfig SelectConfig(string imagesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.SurveyDataSource, SurveyDataSource>()
                .Map(dest => dest.IsActive, src => src.IsActive.HasValue ? src.IsActive.Value : false)
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير مفعل")
                .Map(dest => dest.Image, src => !string.IsNullOrEmpty(src.Image) ? $"{imagesGetPath}/{src.Image}" : $"{imagesGetPath}/noImage.png")
                .Config;
        }


    }
}
