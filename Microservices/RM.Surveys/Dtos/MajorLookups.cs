
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.Surveys.Dtos
{
    public class MajorLookups:BaseDto<MajorLookups,Models.MajorLookup>
    {

        [JsonIgnore]
        public int? Id { get; set; }     
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        public int? MapId { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }


        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.MajorLookup, MajorLookups>()

                .Config;
        }

        public TypeAdapterConfig UpdateConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<MajorLookups, Models.MajorLookup>().IgnoreNullValues(true)
                .Ignore(x=>x.MapId)
                .Config;
        }

        public TypeAdapterConfig AddConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<MajorLookups, Models.MajorLookup>().IgnoreNullValues(true)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }
    }
}
