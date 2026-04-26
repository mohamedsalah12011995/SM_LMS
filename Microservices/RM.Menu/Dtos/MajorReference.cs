using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;


namespace RM.Menu.Dtos
{
    public class MajorReference : BaseDto<MajorReference, Models.ReferencesMajor>
    {
        public MajorReference()
        {
            References = new List<ReferenceDto>();
        }
        [JsonIgnore]
        public int? Id { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public List<ReferenceDto> References { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.ReferencesMajor, MajorReference>()
                .Map(dest => dest.References, src => src.References != null ? src.References.Where(c => c.IsDeleted == !true).Adapt<List<ReferenceDto>>() : new List<ReferenceDto>())

                .Config;
        }

    }
}
