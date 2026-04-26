using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;
using static RM.Users.Dtos.Users;

namespace RM.Users.Dtos
{
    public class MajorReference : BaseDto<MajorReference, Models.ReferencesMajor>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public List<UserReference> References { get; set; }

        public static TypeAdapterConfig PortalSelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.ReferencesMajor, MajorReference>()
                .Map(dest => dest.References, src => src.References.Where(x => x.IsPortal == true).ToList().Adapt<List<UserReference>>(UserReference.SelectConfig()))  
                .Config;

        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.ReferencesMajor, MajorReference>()
                .Map(dest => dest.References, src => src.References.ToList().Adapt<List<UserReference>>(UserReference.SelectConfig()))
                .Config;

        }
    }

    public class MajorReferenceTree
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string Label { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public List<UserReferenceTree> References { get; set; }
        public List<UserReferenceTree> Children { get; set; }
        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.ReferencesMajor, MajorReferenceTree>()
                .Map(dest => dest.Label, src => src.NameAr)
                .Map(dest => dest.References, src => src.References.ToList().Adapt<List<UserReferenceTree>>(UserReferenceTree.SelectConfig(null,null)))
                .Config;

        }
    }

    public class UserReferenceTree
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string Label { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }

        [JsonIgnore]
        public int? ReferencesMajorId { get; set; }
        public string referencesMajorId { set { ReferencesMajorId = Accessor.Set(value); } get { return Accessor.Get(ReferencesMajorId); } }


        public List<ReferenceJobRole> ReferenceJobRole { get; set; }

        public List<AdminMenuUserDto> AdminMenu { get; set; }


        [JsonIgnore]
        public int? ParentId { get; set; }
        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get(ParentId); } }

        public IEnumerable<UserReferenceTree> Children { get; set; }

        public static TypeAdapterConfig SelectConfig(List<ReferenceJobRole> ReferenceJobRole = null, List<AdminMenuUserDto> AdminMenu = null)
        {
            return new TypeAdapterConfig()
                .NewConfig<UserReference, UserReferenceTree>()
                .Map(dest => dest.Label, src => src.NameAr)
                .Map(dest => dest.ReferenceJobRole, src => ReferenceJobRole == null || ReferenceJobRole.Count() == 0 ? src.ReferenceJobRole != null ? src.ReferenceJobRole.Adapt<List<ReferenceJobRole>>(Dtos.Users.ReferenceJobRole.SelectConfig()) : new List<ReferenceJobRole>() : ReferenceJobRole)
                .Map(dest => dest.AdminMenu, src => AdminMenu == null ? src.AdminMenu != null || AdminMenu.Count() == 0 ? src.AdminMenu.Adapt<List<AdminMenuUserDto>>(AdminMenuUserDto.SelectConfig()) : new List<AdminMenuUserDto>() : AdminMenu)

                .Config;

        }
    }
}
