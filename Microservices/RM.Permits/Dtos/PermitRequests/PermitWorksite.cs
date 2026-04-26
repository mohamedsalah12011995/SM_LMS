using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Permits.Dtos
{
    public class PermitWorksite : BaseDto<PermitWorksite, Models.PermitWorkSite>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        [JsonIgnore]
        public int? PermitId { get; set; }

        [JsonIgnore]
        public int? WorksiteId { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        public string Permitid { set { PermitId = Accessor.Set(value); } get { return Accessor.Get<int?>(PermitId); } }

        public string worksiteId { set { WorksiteId = Accessor.Set(value); } get { return Accessor.Get<int?>(WorksiteId); } }

        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.PermitWorkSite, PermitWorksite>()
                .Map(dest => dest.NameAr, src => src.WorksiteNavigation.NameAr != null ? src.WorksiteNavigation.NameAr : string.Empty)
                .Map(dest => dest.NameEn, src => src.WorksiteNavigation.NameEn != null ? src.WorksiteNavigation.NameEn : string.Empty)

                 .Config;
        }

    }
}
