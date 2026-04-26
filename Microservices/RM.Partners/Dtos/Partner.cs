
using LinqKit;
using Mapster;
using RM.Core.Helpers;
using System;
using System.Text.Json.Serialization;

namespace RM.Partners.Dtos
{
    public class Partner
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        [JsonIgnore]
        public int? ActivatedBy { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string PartnershipTitleAr { get; set; }
        public string PartnershipTitleEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string AddressAr { get; set; }
        public string AddressEn { get; set; }
        public string RmdepartmentNameEn { get; set; }
        public string RmdepartmentNameAr { get; set; }
        public string IconUrl { get; set; }
        public string IconUrlBase64 { get; set; }
        public string StatusString { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string PersonCreatedBy { get; set; }

        public bool? ContractActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ContractDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string ContractDateString { get; set; }
        public string ContractStatusEn { get; set; }
        public string ContractStatusAr { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.Partner> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Partner>(true);

                filter.And(u => u.ReferenceId == ReferenceId);
                filter.And(u => u.EntityId == EntityId);

            if (ContractDate.HasValue)
                filter.And(u => u.ContractDate == null || u.ContractDate.Value.Date == ContractDate.Value.Date);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (!string.IsNullOrEmpty(TitleAr))
                filter.And(u => u.TitleAr.Contains(TitleAr));
            if (!string.IsNullOrEmpty(TitleEn))
                filter.And(u => u.TitleEn.Contains(TitleEn));

            if (!string.IsNullOrEmpty(RmdepartmentNameAr))
                filter.And(u => u.RmdepartmentNameAr.Contains(RmdepartmentNameAr));
            if (!string.IsNullOrEmpty(RmdepartmentNameEn))
                filter.And(u => u.RmdepartmentNameEn.Contains(RmdepartmentNameEn));

            if (!string.IsNullOrEmpty(PartnershipTitleAr))
                filter.And(u => u.PartnershipTitleAr.Contains(PartnershipTitleAr));
            if (!string.IsNullOrEmpty(PartnershipTitleEn))
                filter.And(u => u.PartnershipTitleEn.Contains(PartnershipTitleEn));

            if (ContractActive.HasValue)
                filter.And(u => u.ContractActive == ContractActive);

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig(string ImagesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Partner, Partner>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.ContractDateString, src => src.ContractDate.HasValue ? src.ContractDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.IconUrl, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.IconUrl) ? ImagesGetPath + "/" + src.IconUrl : null)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير فعال")
                .Map(dest => dest.ContractStatusAr, src => src.ContractActive == true ? "مفعل" : "غير مفعل")
                .Map(dest => dest.ContractStatusEn, src => src.ContractActive == true ? "Active" : "Not Active")

                    .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Partner, Models.Partner>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Partner, Models.Partner>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsActive, src => false)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }

    }
}

