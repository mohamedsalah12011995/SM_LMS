
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.GovServices.Dtos
{
    public class Govservice : BaseDto<Govservice, Models.GovService>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        [JsonIgnore]
        public int? ActivatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get<int?>(ParentId); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string SiteNameAr { get; set; }
        public string SiteNameEn { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public string AudienceEn { get; set; }
        public string AudienceAr { get; set; }
        public string ServiceChannelEn { get; set; }
        public string ServiceChannelAr { get; set; }
        public string RequirementsEn { get; set; }
        public string RequirementsAr { get; set; }
        public string StepsEn { get; set; }
        public string StepsAr { get; set; }
        public string ServiceUrl { get; set; }
        public string StatusString { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public string DeletedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string CreatedDateString { get; set; }
        public string ActivatedDateString { get; set; }

        public bool? IsRoot { get; set; }
        public bool? IsDeleted { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsActive { get; set; }


        [JsonIgnore]
        public int? LessAverageId { get; set; }
        public string lessAverageId { set { LessAverageId = Accessor.Set(value); } get { return Accessor.Get<int?>(LessAverageId); } }


        [JsonIgnore]
        public int? AverageId { get; set; }
        public string averageId { set { AverageId = Accessor.Set(value); } get { return Accessor.Get<int?>(AverageId); } }


        [JsonIgnore]
        public int? AboveAverageId { get; set; }
        public string aboveAverageId { set { AboveAverageId = Accessor.Set(value); } get { return Accessor.Get<int?>(AboveAverageId); } }

        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.GovService> Filteration()
        {
            var filter = PredicateBuilder.New<Models.GovService>(true);

            if (ReferenceId.HasValue)
                filter.And(u => u.ReferenceId == ReferenceId);

            if (ParentId.HasValue)
                filter.And(u => u.ParentId == ParentId);

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

        public static TypeAdapterConfig SelectConfig(string ImagesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.GovService, Dtos.Govservice>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.ImageUrl, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.ImageUrl) ? ImagesGetPath + "/" + src.ImageUrl : null)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير مفعل")

                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Dtos.Govservice, Models.GovService>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Dtos.Govservice, Models.GovService>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }
    }
}
