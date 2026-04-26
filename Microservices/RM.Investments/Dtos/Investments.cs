
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Models;
using System;
using System.Text.Json.Serialization;

namespace RM.Investments.Dtos
{
    public class Investments:BaseDto<Investments,Models.Investment>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? OpportunityType { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        [JsonIgnore]
        public int? ActivatedBy { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public string opportunityType { set { OpportunityType = Accessor.Set(value); } get { return Accessor.Get<int?>(OpportunityType); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string OpportunityNo { get; set; }
        public string OpportunityTypeName { get; set; }
        public string OpportunityReference { get; set; }
        public string OpportunityUrl { get; set; }
        public string Price { get; set; }
        public string StatusString { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? LastInquiryDate { get; set; }
        public DateTime? LastAdmissionDate { get; set; }
        public DateTime? OpenBidDate { get; set; }
        public DateTime? OpportunityDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? ActivatedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string LastInquiryDateString { get; set; }
        public string LastAdmissionDateString { get; set; }
        public string OpenBidDateString { get; set; }
        public string OpportunityDateString { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<Models.Investment> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Investment>(true);

            filter.And(u => u.ReferenceId == ReferenceId);
            filter.And(u => u.EntityId == EntityId);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (OpportunityDate.HasValue)
                filter.And(u => u.OpportunityDate == null || u.OpportunityDate.Value.Date == OpportunityDate.Value.Date);

            if (!string.IsNullOrEmpty(NameAr))
                filter.And(u => u.NameAr.Contains(NameAr));
            if (!string.IsNullOrEmpty(NameEn))
                filter.And(u => u.NameEn.Contains(NameEn));           
            if (!string.IsNullOrEmpty(Price))
                filter.And(u => u.Price.Contains(Price));

            if (OpportunityType.HasValue)
                filter.And(u => u.OpportunityType == OpportunityType);

            if (!string.IsNullOrEmpty(OpportunityReference))
                filter.And(u => u.OpportunityReference.Contains(OpportunityReference));

            if (!string.IsNullOrEmpty(OpportunityNo))
                filter.And(u => u.OpportunityNo.Contains(OpportunityNo));

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;

        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Investment, Investments>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.LastAdmissionDateString, src => src.LastAdmissionDate.HasValue ? src.LastAdmissionDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.LastInquiryDateString, src => src.LastInquiryDate.HasValue ? src.LastInquiryDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.OpenBidDateString, src => src.OpenBidDate.HasValue ? src.OpenBidDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.OpportunityDateString, src => src.OpportunityDate.HasValue ? src.OpportunityDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.OpportunityTypeName, src => src.OpportunityTypeNavigation != null ? src.OpportunityTypeNavigation.NameAr : string.Empty)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير فعال")

                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Investments, Models.Investment>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Investments, Models.Investment>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsActive, src => false)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }
    }
}
