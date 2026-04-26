using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Users.Dtos
{
    public class AdminMenuUserDto : BaseDto<AdminMenuUserDto, Models.AdminMenu>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        [JsonIgnore]
        public int? ReferencesMajorId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Url { get; set; }
        public string CmsIdentity { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get(ParentId); } }
        public string referencesMajorId { set { ReferencesMajorId = Accessor.Set(value); } get { return Accessor.Get(ReferencesMajorId); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public string userId { set { UserId = Accessor.Set(value); } get { return Accessor.Get(UserId); } }
        public string EntityNameAr { get; set; }
        public string EntityNameEn { get; set; }
        public List<AdminMenuUserDto> SubMenu { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.AdminMenu, AdminMenuUserDto>()
                .Map(dest => dest.SubMenu, src => src.InverseParent != null ? src.InverseParent.Adapt<List<AdminMenuUserDto>>() : new List<AdminMenuUserDto>())

                .Config;
        }



    }
}
