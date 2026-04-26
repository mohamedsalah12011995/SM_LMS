using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Menu.Dtos
{
    public class EntitiesItem : BaseDto<EntitiesItem, Models.Entity>
    {
        public EntitiesItem()
        {
            SubEntities = new List<EntitiesItem>();
        }
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? TypeId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? ReferencesMajorId { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string typeId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get(TypeId); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string referencesMajorId { set { ReferencesMajorId = Accessor.Set(value); } get { return Accessor.Get(ReferencesMajorId); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool? ShowMenuIdentifier { get; set; }
        public List<EntitiesItem> SubEntities { get; set; }


        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Entity, EntitiesItem>()
               .Map(dest => dest.SubEntities, src => src.InverseParent.Any() ? src.InverseParent.Adapt<List<EntitiesItem>>() : new List<EntitiesItem>())

                .Config;
        }

    }
}
