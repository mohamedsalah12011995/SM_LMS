using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Menu.Dtos
{
    public class UserDto : BaseDto<UserDto, Models.User>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ReferenceID { get; set; }
        [JsonIgnore]
        public int? ReferenceMajorId { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string referenceId { set { ReferenceID = Accessor.Set(value); } get { return Accessor.Get(ReferenceID); } }
        public string referenceMajorId { set { ReferenceMajorId = Accessor.Set(value); } get { return Accessor.Get(ReferenceMajorId); } }
        public List<EntitiesPermission> UserEntities { get; set; }

        [JsonIgnore]
        public int? _rootReferenceId { get; set; }
        public string RootReferenceId { set { _rootReferenceId = Accessor.Set(value); } get { return Accessor.Get(_rootReferenceId); } }

        public class EntitiesPermission : BaseDto<EntitiesPermission, Models.UsersEntity>
        {

            [JsonIgnore]
            public int? Id { get; set; }
            public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
            [JsonIgnore]
            public int? UserId { get; set; }
            [JsonIgnore]
            public int? EntityId { get; set; }
            public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
            public string userId { set { UserId = Accessor.Set(value); } get { return Accessor.Get(UserId); } }
            public string NameAr { get; set; }
            public string NameEn { get; set; }
            public override void AddCustomMappings()
            {
                SetCustomMappings();
                SetCustomMappingsInverse()
                    .Map(dest => dest.Id, src => src.EntityId);
            }
        }

        public override void AddCustomMappings()
        {
            SetCustomMappings()

                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now);



            SetCustomMappingsInverse()
                .Map(dest => dest.ReferenceMajorId, src => src.Reference.ReferencesMajorId)
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.ReferenceID, src => src.ReferenceId);



        }

    }
}
