
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Users.Dtos
{
    public class UsersEntityReference : BaseDto<UsersEntityReference, Models.UsersEntityReference>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        [JsonIgnore]
        public int? UserId { get; set; }
        public string userId { set { UserId = Accessor.Set(value); } get { return Accessor.Get(UserId); } }

        [JsonIgnore]
        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }


        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ReferenceUrl { get; set; }


        [JsonIgnore]
        public int? ParentId { get; set; }
        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get(ParentId); } }

        [JsonIgnore]
        public int? ReferenceRootId { get; set; }
        public string referenceRootId { set { ReferenceRootId = Accessor.Set(value); } get { return Accessor.Get(ReferenceRootId); } }


        public bool? Add { get; set; }
        public bool? Edit { get; set; }
        public bool? Delete { get; set; }
        public bool? List { get; set; }
        public bool? Activate { get; set; }
        public bool? Reports { get; set; }
        public bool? View { get; set; }
        public List<EntitiesItems> ReferenceEntities { get; set; }



        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<UsersEntityReference, Models.UsersEntityReference>().IgnoreNullValues(true)
                .Map(dest => dest.UserId, src => userId)

                .Config;
        }


    }
}
