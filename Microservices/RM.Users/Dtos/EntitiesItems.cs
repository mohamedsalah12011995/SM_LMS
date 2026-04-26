using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Users.Dtos
{
    public class EntitiesItems : BaseDto<EntitiesItems, Models.UsersEntity>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? TypeId { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public string typeId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get(TypeId); } }

        [JsonIgnore]
        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public bool? Add { get; set; }
        public bool? Edit { get; set; }
        public bool? Delete { get; set; }
        public bool? List { get; set; }
        public bool? Activate { get; set; }
        public bool? Reports { get; set; }
        public bool? View { get; set; }


        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<EntitiesItems, Models.UsersEntity>().IgnoreNullValues(true)
                .Map(dest => dest.UserId, src => userId)

                .Config;
        }

        public static TypeAdapterConfig SelectConfig(Models.Entity entity)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.UsersEntity, EntitiesItems>()
                .Map(dest => dest.NameAr, src => entity.NameAr)
                .Map(dest => dest.NameEn, src => entity.NameEn)

                .Config;


        }


    }
}
