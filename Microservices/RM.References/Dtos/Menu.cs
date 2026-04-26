using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;


namespace RM.References.Dtos
{
    public class Menu : BaseDto<Menu, Models.Menu>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
        public string Url { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        [JsonIgnore]
        public int? ParentId { get; set; }
        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get(ParentId); } }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsHidden { get; set; }
        public DateTime? CreatedDate { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }
        public int? MenuOrder { get; set; }
        [JsonIgnore]
        public int? ArticleId { get; set; }
        public string articleId { set { ArticleId = Accessor.Set(value); } get { return Accessor.Get(ArticleId); } }

        public List<Menu> subMenus { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public string FontIcon { get; set; }
        public string ImageIcon { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }

        public override void AddCustomMappings()
        {
            SetCustomMappings()

                .Map(dest => dest.CreatedDate, src => DateTime.Now);

            SetCustomMappingsInverse()
                .Map(dest => dest.subMenus, src => src.InverseParent != null ? src.InverseParent.Adapt<List<Menu>>() : new List<Menu>());


        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Menu, Menu>()
                .Map(dest => dest.subMenus, src => src.InverseParent != null ? src.InverseParent.Adapt<List<Menu>>() : new List<Menu>())

                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                  .NewConfig<Menu, Models.Menu>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }

    }
}
