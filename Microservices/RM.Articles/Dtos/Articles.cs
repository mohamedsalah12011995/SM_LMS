using DocumentFormat.OpenXml.Vml;
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using RM.Models;
using System.Text.Json.Serialization;

namespace RM.Articles.Dtos
{
    public class Articles : BaseDto<Articles, Article>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        [JsonIgnore]
        public int? EntityId { get; set; }

        [JsonIgnore]
        public int? SpecificEntityId { get; set; }

        [JsonIgnore]
        public int? ReferenceId { get; set; }

        [JsonIgnore]
        public int? ActivatedBy { get; set; }

        [JsonIgnore]
        public int? DeletedBy { get; set; }

        [JsonIgnore]
        public int? CreatedBy { get; set; }

        [JsonIgnore]
        public int? UpdatedBy { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string specificEntityId { set { SpecificEntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(SpecificEntityId); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string updateddBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string FrontIdentity { get; set; }
        public string StatusString { get; set; }
        public string ArticleUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string ItemUrl { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string MenuCode { get; set; }
        public bool? ShowBySearch { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<Article> Filteration()
        {
            var filter = PredicateBuilder.New<Article>(true);

                filter.And(u => u.ReferenceId == ReferenceId);


            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (!string.IsNullOrEmpty(NameAr))
                filter.And(u => u.NameAr.Contains(NameAr));
            if (!string.IsNullOrEmpty(NameEn))
                filter.And(u => u.NameEn.Contains(NameEn));


            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;

        }

        public static TypeAdapterConfig SelectConfig(bool IsLocal, string GetPath, string IntranetGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Article, Articles>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير مفعل")
                .Map(dest => dest.DescriptionAr, src => Strings.ReplaceUrlsInContent(IsLocal, src.DescriptionAr, GetPath, IntranetGetPath))
                .Map(dest => dest.DescriptionAr, src => Strings.ReplaceUrlsInContent(IsLocal, src.DescriptionAr, GetPath, IntranetGetPath))
                .Map(dest => dest.ContentAr, src => Strings.ReplaceUrlsInContent(IsLocal, src.ContentAr, GetPath, IntranetGetPath))
                .Map(dest => dest.ContentEn, src => Strings.ReplaceUrlsInContent(IsLocal, src.ContentEn, GetPath, IntranetGetPath))
                .Map(dest => dest.ArticleUrl, src => src.Entity != null ? "/" + src.Entity.FrontIdentity + (src.FrontIdentity != null ? "/" + src.FrontIdentity : "") + "/" + src.Id : null)
                    .Config;
        }

        public  TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Articles, Article>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Articles, Article>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsActive, src => false)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }
    }
}
