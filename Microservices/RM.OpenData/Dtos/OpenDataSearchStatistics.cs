

using DocumentFormat.OpenXml.Bibliography;
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.OpenData.Dtos
{
    public class OpenDataSearchStatistics:BaseDto<OpenDataSearchStatistics,Models.OpenDataStatistics>
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

        public int? FromYear { get; set; }
        public int? ToYear { get; set; }

        public int? Month { get; set; }
        public int? FromMonth { get; set; }
        public int? ToMonth { get; set; }

        public int? DayCount { get; set; }
        public int? MonthCount { get; set; }
        public int? YearCount { get; set; }

        public bool? IsGregorian { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? FromCreatedDate { get; set; }

        public DateTime? ToCreatedDate { get; set; }

        public string CreatedDateString { get; set; }


        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }

        [JsonIgnore]
        public int? StatisticTypeId { get; set; }
        public string statisticTypeId { set { StatisticTypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(StatisticTypeId); } }
        public string StatisticTypeAr { get; set; }
        public string StatisticTypeEn { get; set; }

        public List<string> Emails { get; set; } = new List<string>();

        public string Subject { get; set; }
        public string Body { get; set; }
        public string FileName { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.OpenDataStatistics> Filteration()
        {
            var filter = PredicateBuilder.New<Models.OpenDataStatistics>(true);

            filter.And(u => u.ReferenceId == ReferenceId);

            if (StatisticTypeId.HasValue)
                filter.And(u => u.StatisticType == StatisticTypeId);

            if (ParentTypeId.HasValue)
                filter.And(u => u.Type.ParentId == ParentTypeId);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (FromCreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date >= FromCreatedDate.Value.Date);

            if (ToCreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date <= ToCreatedDate.Value.Date);

            if (DistrictId.HasValue)
                filter.And(u => u.DistrictId == DistrictId);

            if (TypeId.HasValue)
                filter.And(u => u.TypeId == TypeId);

            if (FromYear.HasValue)
                filter.And(u => u.FromYear == FromYear);

            if (ToYear.HasValue)
                filter.And(u => u.ToYear == ToYear);

            if (FromMonth.HasValue)
                filter.And(u => u.FromMonth == FromMonth);

            if (ToMonth.HasValue)
                filter.And(u => u.ToMonth == ToMonth);

            if (IsGregorian.HasValue)
                filter.And(u => u.IsGregorian == IsGregorian);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.OpenDataStatistics, OpenDataSearchStatistics>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.DistrictName, src => src.District != null ? src.District.NameAr : null)
                .Map(dest => dest.DistrictNameAr, src => src.District != null ? src.District.NameAr : null)
                .Map(dest => dest.DistrictNameEn, src => src.District != null ? src.District.NameEn : null)
                .Map(dest => dest.TypeName, src => src.Type != null ? src.Type.NameAr : null)
                .Map(dest => dest.TypeNameAr, src => src.Type != null ? src.Type.NameAr : null)
                .Map(dest => dest.TypeNameEn, src => src.Type != null ? src.Type.NameEn : null)
                    .Config;
        }

        public TypeAdapterConfig UpdateConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<OpenDataSearchStatistics, Models.OpenDataStatistics>().IgnoreNullValues(true)
                .Config;
        }

        public TypeAdapterConfig AddConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<OpenDataSearchStatistics, Models.OpenDataStatistics>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)

                .Config;
        }
    }

}
