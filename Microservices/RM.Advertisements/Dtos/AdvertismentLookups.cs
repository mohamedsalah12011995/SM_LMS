using Mapster;
using RM.Core.CommonDtos;
using  RM.Core.Helpers;
using RM.Models;
using System.Text.Json.Serialization;

namespace RM.Advertisements.Dtos
{
    public class AdvertismentLookups:BaseDto<AdvertismentLookups, MajorLookup>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<MajorLookup, AdvertismentLookups>()
                .Config;
        }
    }
}
