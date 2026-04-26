using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers; 
using System.Text.Json.Serialization;

namespace RM.Advertisements.Dtos
{
    public class Reference : BaseDto<Reference, Models.Reference>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Reference, Reference>()                 
                    .Config;
        }
    }
}
