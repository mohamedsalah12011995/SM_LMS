
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System;
using System.Text.Json.Serialization;

namespace RM.GovServices.Dtos
{
    public class Eservices:BaseDto<Eservices,Models.Eservice>
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
        public int? UpdatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? ActivatedBy { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get<int?>(ParentId); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Path { get; set; }
        public string ContentEn { get; set; }
        public string ContentAr { get; set; }
        public string DeliveryChannelAr { get; set; }
        public string DeliveryChannelEn { get; set; }
        public string ServiceLink { get; set; }
        public string ServiceUrl { get; set; }
        public string CategoryAr { get; set; }
        public string CategoryEn { get; set; }
        public string TypeAr { get; set; }
        public string TypeEn { get; set; }
        public string Maturity { get; set; }
        public string FeesAr { get; set; }
        public string FeesEn { get; set; }
        public string FeesInk { get; set; }
        public string PaymentChannelAr { get; set; }
        public string PaymentChannelEn { get; set; }
        public string ExecutionTime { get; set; }
        public string ExecutionTimeEn { get; set; }

        public string ContactEn { get; set; }
        public string ContactAr { get; set; }
        public string RequirementsAr { get; set; }
        public string RequirementsEn { get; set; }
        public string AttachmentsAr { get; set; }
        public string AttachmentsEn { get; set; }
        public string ProceduresAr { get; set; }
        public string ProceduresEn { get; set; }
        public string UserGuid { get; set; }
        public string StatusString { get; set; }
        public string IconUrl { get; set; }
        public bool? IsRoot { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ActivatedDate { get; set; }


        public string DeletedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string CreatedDateString { get; set; }
        public string ActivatedDateString { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.Eservice> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Eservice>(true);

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
                .NewConfig<Models.Eservice, Dtos.Eservices>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.IconUrl, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.IconUrl) ? ImagesGetPath + "/" + src.IconUrl : null)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير مفعل")

                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Dtos.Eservices, Models.Eservice>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Dtos.Eservices, Models.Eservice>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.IsRoot, src => false)
                .Config;
        }
    }
}
