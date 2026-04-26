using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;


namespace RM.Surveys.Dtos
{
    public class Reference : BaseDto<Reference, Models.Reference>
    {
        [JsonIgnore]
        public int? _id { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }


        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Reference, Reference>()
                .Map(dest => dest._id, src => src.Id)
                .Config;
        }
    }
}
