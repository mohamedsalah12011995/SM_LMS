using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Lookups.Dtos
{
    public class MajorLookupsType : BaseDto<MajorLookupsType, Models.MajorLookupsType>
    {
        public MajorLookupsType()
        {
            MajorLookups = new List<MajorLookups>();
        }
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool? IsActive { get; set; }


        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public List<MajorLookups> MajorLookups { get; set; }
        public string StatusStringAr { get; set; }
        public string StatusStringEn { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<Models.MajorLookupsType> Filteration()
        {
            var filter = PredicateBuilder.New<Models.MajorLookupsType>(true);

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            if (!string.IsNullOrEmpty(NameAr))
                filter.And(u => u.NameAr.Contains(NameAr));
            if (!string.IsNullOrEmpty(NameEn))
                filter.And(u => u.NameEn.Contains(NameEn));

            return filter;
        }

        public static TypeAdapterConfig SelectConfig(int? ReferenceId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.MajorLookupsType, MajorLookupsType>()
                .Map(dest => dest.StatusStringAr, src => src.IsActive == true ? "فعال" : "غير فعال")
                .Map(dest => dest.StatusStringEn, src => src.IsActive == true ? "Active" : "Deactive")
                .Map(dest => dest.MajorLookups, src => src.MajorLookups != null ? src.MajorLookups.Where(c => c.ParentId == null && c.IsDeleted != true).Where(c => ReferenceId != null ? c.ReferenceId == ReferenceId : true).ToList().Adapt<List<Dtos.MajorLookups>>(Dtos.MajorLookups.SelectConfig()) : new List<MajorLookups>())

                    .Config;
        }

        public TypeAdapterConfig UpdateConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<MajorLookupsType, Models.MajorLookupsType>().IgnoreNullValues(true)
                .Config;
        }

        public TypeAdapterConfig AddConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<MajorLookupsType, Models.MajorLookupsType>().IgnoreNullValues(true)
                .Map(dest => dest.IsActive, src => true)
                .Config;
        }
    }
}
