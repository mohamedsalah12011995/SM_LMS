using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Models;

namespace RM.ExternalSites.Dtos
{
    public class ExternalSites:BaseDto<ExternalSites,ExternalSite>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? ActivatedBy { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get<int?>(ParentId); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string Image { get; set; }
        public string ImageBase64 { get; set; }
        public string Url { get; set; }
        public string UpdatedPerson { get; set; }
        public string CreatedPerson { get; set; }
        public string ParentName { get; set; }
        public string StatusString { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? ActivatedDate { get; set; }


        public string ActivatedDateString { get; set; }
        public string CreatedDateString { get; set; }
        public string DeletedDateString { get; set; }
        public string UpdatedDateString { get; set; }

        public List<ExternalSites> Siteslists { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<ExternalSite> Filteration()
        {
            var filter = PredicateBuilder.New<ExternalSite>(true);

            if (ReferenceId.HasValue)
                filter.And(u => u.ReferenceId == ReferenceId);


            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (!string.IsNullOrEmpty(NameAr))
                filter.And(u => u.NameAr.Contains(NameAr));
            if (!string.IsNullOrEmpty(NameEn))
                filter.And(u => u.NameEn.Contains(NameEn));            
            
            if (!string.IsNullOrEmpty(CreatedPerson))
                filter.And(u => u.CreatedByNavigation.Name.Contains(CreatedPerson));
            if (!string.IsNullOrEmpty(UpdatedPerson))
                filter.And(u => u.UpdatedByNavigation.Name.Contains(UpdatedPerson));

            if (ParentId.HasValue)
                filter.And(u => u.ParentId == ParentId);            
            
            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;

        }

        public static TypeAdapterConfig SelectConfig(string GetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<ExternalSite, ExternalSites>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.CreatedPerson, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.UpdatedPerson, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير مفعل")
                .Map(dest => dest.ParentName, src => src.Parent != null ? src.Parent.NameAr : null)
                .Map(dest => dest.Siteslists, src => src.InverseParent!=null? src.InverseParent.Adapt<List<ExternalSites>>(SelectConfig(GetPath)):new List< ExternalSites>())

                //.Map(dest => dest.ContentAr, src => Strings.ReplaceUrlsInContent(IsLocal, src.ContentAr, GetPath, IntranetGetPath))
                //.Map(dest => dest.ContentEn, src => Strings.ReplaceUrlsInContent(IsLocal, src.ContentEn, GetPath, IntranetGetPath))
                .Map(dest => dest.Image, src => !Strings.CheckStringNullOrEmptyOrWhiteSpace(src.Image) ? GetPath + "/" + src.Image : null)
                    .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<ExternalSites, ExternalSite>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<ExternalSites, ExternalSite>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsActive, src => false)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.ParentId, src => src.ParentId == null ? 2 : src.ParentId)
                .Config;
        }
    }
}
