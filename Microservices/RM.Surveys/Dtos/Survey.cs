
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Surveys.Interfaces;
using System.Text.Json.Serialization;

namespace RM.Surveys.Dtos
{

    public class Survey : BaseDto<Survey, Models.Survey>, IFilterationSurvey<Models.Survey>
    {
        public Survey()
        {
            GroupQuestion = new List<GroupQuestion>();
            PublishEntity = new List<Reference>();
        }
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? ActivatedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }

        [JsonIgnore]
        public int? ThemeId { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string referenceID { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string entityID { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public string createdById { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }
        public string deletedById { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get(DeletedBy); } }
        public string activatedById { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get(ActivatedBy); } }
        public string updatedById { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get(UpdatedBy); } }

        public string themeId { set { ThemeId = Accessor.Set(value); } get { return Accessor.Get(ThemeId); } }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string StatusString { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string Code { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public bool? IsExpired { get; set; }
        public bool? IsStarted { get; set; }
        public bool? ShowInHomePage { get; set; }
        public bool? UseCapcha { get; set; }
        public bool? InnerOnly { get; set; }

        public string ThemeName { get; set; }

        public DateTime? FromDate { get; set; }

        public string FromDateString { get; set; }

        public DateTime? ToDate { get; set; }

        public string Image { get; set; }
        public string ImageBase64 { get; set; }
        public string ToDateString { get; set; }
        public int? AnswersCount { get; set; }
        public List<ReferenceShareUrl> ReferenceShareUrls { get; set; } = new List<ReferenceShareUrl>();
        public List<Reference> PublishEntity { get; set; }
        public List<GroupQuestion> GroupQuestion { get; set; }
        public List<CronSettings> CronSettings { get; set; } = new List<CronSettings>();

        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<Models.Survey> Filteration(List<int> publishedSurveys)
        {
            var filter = PredicateBuilder.New<Models.Survey>(true);

            if (ReferenceId != null)
                filter.And(u => u.ReferenceId == ReferenceId || publishedSurveys.Contains(u.Id));


            if (!string.IsNullOrEmpty(TitleAr))
                filter.And(u => u.TitleAr.Contains(TitleAr));
            if (!string.IsNullOrEmpty(TitleEn))
                filter.And(u => u.TitleEn.Contains(TitleEn));


            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (FromDate.HasValue)
                filter.And(u => u.FromDate.Value.Date == FromDate.Value.Date);

            if (ToDate.HasValue)
                filter.And(u => u.ToDate.Value.Date == ToDate.Value.Date);

            if (ShowInHomePage.HasValue)
                filter.And(u => u.ShowInHomePage == ShowInHomePage);

            if (UseCapcha.HasValue)
                filter.And(u => u.UseCapcha == UseCapcha);

            if (InnerOnly.HasValue)
                filter.And(u => u.InnerOnly == InnerOnly);

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig(string imagesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Survey, Survey>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير مفعل")
                .Map(dest => dest.FromDateString, src => src.FromDate.HasValue ? src.FromDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.ToDateString, src => src.ToDate.HasValue ? src.ToDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.IsExpired, src => src.ToDate.HasValue ? src.ToDate.Value < DateTime.Now : false)
                .Map(dest => dest.IsStarted, src => src.FromDate.HasValue ? src.FromDate.Value <= DateTime.Now : true)
                .Map(dest => dest.AnswersCount, src => src.SurveyAnswerActions.Any() ? src.SurveyAnswerActions.Count().ToString() : null)

                .Map(dest => dest.Image, src => !string.IsNullOrEmpty(src.Image) ? $"{imagesGetPath}/{src.Image}" : $"{imagesGetPath}/noImage.png")
                .Map(dest => dest.CronSettings, src => src.CronSettings != null ? src.CronSettings.Adapt<List<Dtos.CronSettings>>(Dtos.CronSettings.SelectConfig()) : new List<Dtos.CronSettings>())
                .Config;
        }

    }

    public class ReferenceShareUrl
    {
        [JsonIgnore]
        public int? _referenceId { get; set; }
        public string ReferenceId { set { _referenceId = Accessor.Set(value); } get { return Accessor.Get(_referenceId); } }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ReviewUrlTitleAr { get; set; }
        public string ReviewUrlTitleEn { get; set; }
        public string ShareUrl { get; set; }
        public string ShareReviewUrl { get; set; }
    }
}




