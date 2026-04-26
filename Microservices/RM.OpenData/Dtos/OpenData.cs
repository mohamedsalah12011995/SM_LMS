

using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.OpenData.Dtos
{
    public class OpenData:BaseDto<OpenData,Models.OpenData>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        [JsonIgnore]
        public int? DistrictId { get; set; }
        public string districtId { set { DistrictId = Accessor.Set(value); } get { return Accessor.Get<int?>(DistrictId); } }
        public string DistrictName { get; set; }
        public string DistrictNameAr { get; set; }
        public string DistrictNameEn { get; set; }

        [JsonIgnore]
        public int? TypeId { get; set; }
        public string typeId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(TypeId); } }
        public string TypeName { get; set; }
        public string TypeNameAr { get; set; }
        public string TypeNameEn { get; set; }
        [JsonIgnore]
        public int? ParentTypeId { get; set; }
        public string parentTypeId { set { ParentTypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(ParentTypeId); } }
        public string ParentTypeName { get; set; }

        public int? Year { get; set; }
        public int? FromYear { get; set; }
        public int? ToYear { get; set; }

        public int? Month { get; set; }
        public int? FromMonth { get; set; }
        public int? ToMonth { get; set; }

        public double? Value { get; set; }

        public bool? IsConfirm { get; set; }
        public string IsConfirmStringAr { get; set; }
        public string IsConfirmStringEn { get; set; }

        [JsonIgnore]
        public int? CreatedBy { get; set; }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        [JsonIgnore]
        public int? ModifiedBy { get; set; }
        public string modifiedBy { set { ModifiedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ModifiedBy); } }
        public DateTime? ModifiedDate { get; set; }

        public DateTime? FromModifiedDate { get; set; }
        public DateTime? ToModifiedDate { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? FromCreatedDate { get; set; }

        public DateTime? ToCreatedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string ModifiedDateString { get; set; }
        public string PersonCreatedBy { get; set; }

        public string PersonModifiedBy { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        [JsonIgnore]
        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }

        public int? StatisticType { get; set; }
        public string StatisticTypeString { get; set; }
        public bool? IsGregorian { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<Models.OpenData> Filteration()
        {
            var filter = PredicateBuilder.New<Models.OpenData>(true);

                filter.And(u => u.ReferenceId == ReferenceId);

            if(ParentTypeId.HasValue)
                filter.And(u => u.Type.ParentId == ParentTypeId);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (FromCreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date >= FromCreatedDate.Value.Date);
            
            if (ToCreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date <= ToCreatedDate.Value.Date);

            if (ModifiedDate.HasValue)
                filter.And(u => u.ModifiedDate == null || u.ModifiedDate.Value.Date == ModifiedDate.Value.Date);

            if (FromModifiedDate.HasValue)
                filter.And(u => u.ModifiedDate == null || u.ModifiedDate.Value.Date >= FromModifiedDate.Value.Date);

            if (ToModifiedDate.HasValue)
                filter.And(u => u.ModifiedDate == null || u.ModifiedDate.Value.Date <= ToModifiedDate.Value.Date);

            if (DistrictId.HasValue)
                filter.And(u => u.DistrictId == DistrictId);

            if (TypeId.HasValue)
                filter.And(u => u.TypeId == TypeId);

            if (Year.HasValue)
                filter.And(u => u.Year == Year);

            if (FromYear.HasValue)
                filter.And(u => u.Year >= FromYear);

            if (ToYear.HasValue)
                filter.And(u => u.Year <= ToYear);

            if (Month.HasValue)
                filter.And(u => u.Year == Year);

            if (FromMonth.HasValue)
                filter.And(u => u.Month >= FromMonth);

            if (ToMonth.HasValue)
                filter.And(u => u.Month <= ToMonth);

            if (Value.HasValue)
                filter.And(u => u.Value == Value);

            if (IsGregorian.HasValue)
                filter.And(u => u.IsGregorian == IsGregorian);

            return filter;
        }

        public ExpressionStarter<Models.OpenDataTemp> FilterationDataTemp()
        {
            var filter = PredicateBuilder.New<Models.OpenDataTemp>(true);

            filter.And(u => u.ReferenceId == ReferenceId);

            if (ParentTypeId.HasValue)
                filter.And(u => u.Type.ParentId == ParentTypeId);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (FromCreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date >= FromCreatedDate.Value.Date);

            if (ToCreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date <= ToCreatedDate.Value.Date);

            if (ModifiedDate.HasValue)
                filter.And(u => u.ModifiedDate == null || u.ModifiedDate.Value.Date == ModifiedDate.Value.Date);

            if (FromModifiedDate.HasValue)
                filter.And(u => u.ModifiedDate == null || u.ModifiedDate.Value.Date >= FromModifiedDate.Value.Date);

            if (ToModifiedDate.HasValue)
                filter.And(u => u.ModifiedDate == null || u.ModifiedDate.Value.Date <= ToModifiedDate.Value.Date);

            if (DistrictId.HasValue)
                filter.And(u => u.DistrictId == DistrictId);

            if (TypeId.HasValue)
                filter.And(u => u.TypeId == TypeId);

            if (Year.HasValue)
                filter.And(u => u.Year == Year);

            if (FromYear.HasValue)
                filter.And(u => u.Year >= FromYear);

            if (ToYear.HasValue)
                filter.And(u => u.Year <= ToYear);

            if (Month.HasValue)
                filter.And(u => u.Year == Year);

            if (FromMonth.HasValue)
                filter.And(u => u.Month >= FromMonth);

            if (ToMonth.HasValue)
                filter.And(u => u.Month <= ToMonth);

            if (Value.HasValue)
                filter.And(u => u.Value == Value);

            if (IsGregorian.HasValue)
                filter.And(u => u.IsGregorian == IsGregorian);

            if (IsConfirm.HasValue)
                filter.And(u => u.IsConfirm == IsConfirm);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.OpenData, OpenData>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.ModifiedDateString, src => src.ModifiedDate.HasValue ? src.ModifiedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonModifiedBy, src => src.ModifiedByNavigation != null ? src.ModifiedByNavigation.Name : string.Empty)
                .Map(dest => dest.DistrictName, src => src.District != null ? src.District.NameAr : null)
                .Map(dest => dest.DistrictNameAr, src => src.District != null ? src.District.NameAr : null)
                .Map(dest => dest.DistrictNameEn, src => src.District != null ? src.District.NameEn : null)
                .Map(dest => dest.TypeName, src => src.Type != null ? src.Type.NameAr : null)
                .Map(dest => dest.TypeNameAr, src => src.Type != null ? src.Type.NameAr : null)
                .Map(dest => dest.TypeNameEn, src => src.Type != null ? src.Type.NameEn : null)
                    .Config;
        }

        public static TypeAdapterConfig SelectDataTempConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.OpenDataTemp, OpenData>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.ModifiedDateString, src => src.ModifiedDate.HasValue ? src.ModifiedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonModifiedBy, src => src.ModifiedByNavigation != null ? src.ModifiedByNavigation.Name : string.Empty)
                .Map(dest => dest.DistrictName, src => src.District != null ? src.District.NameAr : null)
                .Map(dest => dest.DistrictNameAr, src => src.District != null ? src.District.NameAr : null)
                .Map(dest => dest.DistrictNameEn, src => src.District != null ? src.District.NameEn : null)
                .Map(dest => dest.TypeName, src => src.Type != null ? src.Type.NameAr : null)
                .Map(dest => dest.TypeNameAr, src => src.Type != null ? src.Type.NameAr : null)
                .Map(dest => dest.TypeNameEn, src => src.Type != null ? src.Type.NameEn : null)
                .Map(dest => dest.IsConfirm, src => src.IsConfirm.HasValue ? src.IsConfirm : false)
                .Map(dest => dest.IsConfirmStringAr, src => src.IsConfirm == true ? "نعم" : "لا")
                .Map(dest => dest.IsConfirmStringEn, src => src.IsConfirm == true ? "Yes" : "No")
                    .Config;

        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<OpenData, Models.OpenData>().IgnoreNullValues(true)
                .Map(dest => dest.ModifiedBy, src => userId)
                .Map(dest => dest.ModifiedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig UpdateDataTempConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<OpenData, Models.OpenDataTemp>().IgnoreNullValues(true)
                .Map(dest => dest.ModifiedBy, src => userId)
                .Map(dest => dest.ModifiedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<OpenData, Models.OpenData>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)

                .Config;
        }
        public TypeAdapterConfig AddDataTempConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<OpenData, Models.OpenDataTemp>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)

                .Config;
        }
    }

}
