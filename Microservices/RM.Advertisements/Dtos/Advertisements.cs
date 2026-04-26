using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Models;
using System.Text.Json.Serialization;

namespace RM.Advertisements.Dtos
{
    public class Advertisements : BaseDto<Advertisements, Advertisement>
    {
        public Advertisements()
        {
            PublishEntity = new List<Reference>();
        }
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? RegionId { get; set; }

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
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public string regionId { set { RegionId = Accessor.Set(value); } get { return Accessor.Get<int?>(RegionId); } }


        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string ImageUrl { get; set; }
        public string ImageUrlBase64 { get; set; }
        public string Url { get; set; }
        public string FileUrl { get; set; }
        public string FileUrlBase64 { get; set; }

        public string CreatedPerson { get; set; }
        public string UpdatedPerson { get; set; }

        public string StatusString { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? AdvertisementOrder { get; set; }
        public double AdvertisementPeriod { get; set; }
        public string StatusStringEn { get; set; }
        public string ExpiredStringAr { get; set; }
        public string ExpiredStringEn { get; set; }
        public string FromDateString { get; set; }
        public string ToDateString { get; set; }
        public string RegionNameAr { get; set; }
        public string RegionNameEn { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsPopup { get; set; } = false;
        public string IsPopupString { get; set; }
        public bool? IsHomeSliderAd { get; set; } = false;
        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }

        public int? SerialNum { get; set; }
        public string Code { get; set; }
        public string Destination { get; set; }
        public List<Reference> PublishEntity { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Advertisement> Filteration(List<int> publishedAdvertisments)
        {
            var filter = PredicateBuilder.New<Advertisement>(true);

            if (publishedAdvertisments != null)
            {
                foreach (var id in publishedAdvertisments)
                {
                    int tempId = id; // To avoid closure issue
                    filter.Or(u => u.Id == tempId);
                }
                filter.Or(u => u.ReferenceId == ReferenceId);
            }
            else
                filter.And(u => u.ReferenceId == ReferenceId);


            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (FromDate.HasValue)
                filter.And(u => u.FromDate == null || u.FromDate.Value.Date >= FromDate.Value.Date);

            if (ToDate.HasValue)
                filter.And(u => u.ToDate == null || u.ToDate.Value.Date <= ToDate.Value.Date);

            if (!string.IsNullOrEmpty(TitleAr))
                filter.And(u => u.TitleAr.Contains(TitleAr));
            if (!string.IsNullOrEmpty(TitleEn))
                filter.And(u => u.TitleEn.Contains(TitleEn));

            if (IsPopup.HasValue)
                filter.And(u => u.IsPopup == IsPopup);

            if (IsHomeSliderAd.HasValue)
                filter.And(u => u.IsHomeSliderAd == IsHomeSliderAd);

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.EntityId == EntityId);

            filter.And(u => u.IsDeleted != true);

            return filter;

        }

        public ExpressionStarter<Advertisement> PlatformFilteration()
        {
            var filter = PredicateBuilder.New<Advertisement>(true);

            filter.And(u => u.ReferenceId == ReferenceId);
            filter.And(u => u.EntityId == (int)Enums.Entities.Platforms);

            if (RegionId != null)
                filter.And(u => u.RegionId == null || u.RegionId == RegionId);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (FromDate.HasValue)
                filter.And(u => u.FromDate == null || u.FromDate.Value.Date == FromDate.Value.Date);

            if (ToDate.HasValue)
                filter.And(u => u.ToDate == null || u.ToDate.Value.Date == ToDate.Value.Date);

            if (!string.IsNullOrEmpty(TitleAr))
                filter.And(u => u.TitleAr.Contains(TitleAr));
            if (!string.IsNullOrEmpty(TitleEn))
                filter.And(u => u.TitleEn.Contains(TitleEn));

            if (!string.IsNullOrEmpty(Code))
                filter.And(u => u.Code.Contains(Code));
            if (!string.IsNullOrEmpty(Destination))
                filter.And(u => u.Destination.Contains(Destination));

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);


            filter.And(u => u.IsDeleted != true);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig(string ImagesGetPath, List<PublishEntities> publishReferences)
        {
            return new TypeAdapterConfig()
                .NewConfig<Advertisement, Advertisements>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.CreatedPerson, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.UpdatedPerson, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير مفعل")
                .Map(dest => dest.IsPopupString, src => src.IsPopup == true ? "منبثق" : "غير منبثق")
                .Map(dest => dest.ImageUrl, src => !String.IsNullOrEmpty(src.ImageUrl) ? ImagesGetPath + "/" + src.ImageUrl : ImagesGetPath + "/noImage.png")
                .Map(dest => dest.PublishEntity, src => publishReferences != null ? publishReferences.Select(x => new Dtos.Reference { Id = x.ReferenceId }).ToList() : null)
                .Config;
        }

        public static TypeAdapterConfig SelectPlatformConfig(string FilesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Advertisement, Advertisements>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.CreatedPerson, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.UpdatedPerson, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير مفعل")
                .Map(dest => dest.StatusStringEn, src => src.IsActive == true ? "Active" : "Inactive")
                .Map(dest => dest.FileUrl, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.FileUrl) ? FilesGetPath + "/" + src.FileUrl : "")
                .Map(dest => dest.RegionNameAr, src => src.Region != null ? src.Region.NameAr : string.Empty)
                .Map(dest => dest.RegionNameEn, src => src.Region != null ? src.Region.NameEn : string.Empty)
                .Map(dest => dest.ExpiredStringAr, src => src.ToDate.Value.Date < DateTime.Now.Date ? "منتهى" : "سارى")
                .Map(dest => dest.ExpiredStringEn, src => src.ToDate.Value.Date < DateTime.Now.Date ? "Expired" : "Active")
                .Map(dest => dest.FromDateString, src => src.FromDate.GetValueOrDefault().ToString("yyyy-MM-dd"))
                .Map(dest => dest.ToDateString, src => src.ToDate.GetValueOrDefault().ToString("yyyy-MM-dd"))
                .Map(dest => dest.AdvertisementPeriod, src => src.ToDate.Value.Subtract(src.FromDate.Value).TotalDays)
                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Advertisements, Advertisement>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Advertisements, Advertisement>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsActive, src => false)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }

    }
}
