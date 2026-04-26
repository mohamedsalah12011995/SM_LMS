using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;


namespace RM.Menu.Dtos
{
    public class MenuDto : BaseDto<MenuDto, Models.Menu>
    {
        public MenuDto()
        {
            SubMenus = new List<MenuDto>();
        }
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        [JsonIgnore]
        public int? RootParentId { get; set; }


        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? ArticleId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? ReferenceMajorId { get; set; }
        [JsonIgnore]
        public int? TypeId { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get(ParentId); } }
        public string rootParentId { set { RootParentId = Accessor.Set(value); } get { return Accessor.Get(RootParentId); } }


        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }
        public string articleId { set { ArticleId = Accessor.Set(value); } get { return Accessor.Get(ArticleId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public string referenceMajorId { set { ReferenceMajorId = Accessor.Set(value); } get { return Accessor.Get(ReferenceMajorId); } }
        public string typeId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get(TypeId); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
        public string Url { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsHidden { get; set; }
        public bool IsCms { get; set; } = false;
        public bool? IsFirstRoot { get; set; }
        public bool? OpenBlankPage { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? MenuOrder { get; set; }
        public string FontIcon { get; set; }
        public string ImageIcon { get; set; }
        public string SvgIcon { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string ArticleNameAr { get; set; }
        public string EntityNameAr { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool IsParentDifferent { get; set; }
        public MenuDto Parent { get; set; }
        public List<MenuDto> SubMenus { get; set; }


        public static TypeAdapterConfig SelectConfig(bool? isCms)
        {

            return new TypeAdapterConfig()
                .NewConfig<Models.Menu, MenuDto>().IgnoreNullValues(true)
               .Map(dest => dest.OpenBlankPage, src => src.OpenBlankPage ?? false)
                .Map(dest => dest.EntityNameAr, src => src.EntityId.HasValue ? src.Entity.NameAr : null)
                .Map(dest => dest.Url, src => isCms == true ? src.Url : src.Entity != null && src.Entity.FrontIdentity != "" ? "/" + src.Entity.FrontIdentity + (src.Article.FrontIdentity != null ? "/" + src.Article.FrontIdentity : "") + (src.ArticleId.HasValue ? "/" + Accessor.Get<int?>(src.ArticleId) : src.Url.Trim() != "" ? "/" + src.Url : "") : src.Url)
                .Map(dest => dest.SubMenus, src => src.InverseParent.Count > 0 ? src.InverseParent.Adapt<List<MenuDto>>() : new List<MenuDto>())
                .Config;
        }

        public static TypeAdapterConfig SelectConfigForGetMenuById(bool? isCms)
        {

            return new TypeAdapterConfig()
                .NewConfig<Models.Menu, MenuDto>().IgnoreNullValues(true)
               .Map(dest => dest.OpenBlankPage, src => src.OpenBlankPage ?? false)
                .Map(dest => dest.EntityNameAr, src => src.EntityId.HasValue ? src.Entity.NameAr : null)
                .Map(dest => dest.Url, src => isCms == true ? src.Url : src.Entity != null && src.Entity.FrontIdentity != "" ? "/" + src.Entity.FrontIdentity : src.Url)
                .Map(dest => dest.SubMenus, src => src.InverseParent.Count > 0 ? src.InverseParent.Adapt<List<MenuDto>>() : new List<MenuDto>())
                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<MenuDto, Models.Menu>().IgnoreNullValues(true)
                .Map(dest => dest.IsFirstRoot, src => src.ParentId == null ? true : false)
                 .Map(dest => dest.FontIcon, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.FontIcon) ? src.FontIcon : null)
                .Map(dest => dest.ImageIcon, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.ImageIcon) ? src.ImageIcon : null)
                .Map(dest => dest.SvgIcon, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.SvgIcon) ? src.SvgIcon : null)
                .Map(dest => dest.EntityId, src => src.ArticleId.HasValue ? (int)Enums.Entities.Articles : src.EntityId)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<MenuDto, Models.Menu>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.Code, src => Strings.RandomDigits(10).ToString())
                .Map(dest => dest.IsFirstRoot, src => src.ParentId == null ? true : false)
                .Map(dest => dest.FontIcon, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.FontIcon) ? src.FontIcon : null)
                .Map(dest => dest.ImageIcon, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.ImageIcon) ? src.ImageIcon : null)
                .Map(dest => dest.SvgIcon, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.SvgIcon) ? src.SvgIcon : null)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.IsHidden, src => false)
                .Map(dest => dest.EntityId, src => src.ArticleId.HasValue ? (int)Enums.Entities.Articles : src.EntityId)

                .Config;
        }



    }
    public class MenuTree
    {
        public MenuTree()
        {
            Children = new List<MenuTree>();
        }
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? _key { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]

        public int? ArticleId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? ReferenceMajorId { get; set; }
        [JsonIgnore]
        public int? _parentId { get; set; }
        public string Label { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Icon { get; set; }
        public string LabelEn { get; set; }
        public string Code { get; set; }
        public string Url { get; set; }
        public int? MenuOrder { get; set; }
        public string FontIcon { get; set; }
        public string ImageIcon { get; set; }
        public string SvgIcon { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string ArticleNameAr { get; set; }
        public string EntityNameAr { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsHidden { get; set; }
        public bool expanded { get; set; } = true;
        public bool selectable { get; set; }
        public string Key { set { _key = Accessor.Set(value); } get { return Accessor.Get(_key); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }

        public string ParentId { set { _parentId = Accessor.Set(value); } get { return Accessor.Get(_parentId); } }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public string articleId { set { ArticleId = Accessor.Set(value); } get { return Accessor.Get(ArticleId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public bool? OpenBlankPage { get; set; } = false;
        public string ParentNameAr { get; set; }
        public string ParentNameEn { get; set; }
        public string ReferenceUrl { get; set; }

        public MenuTree Parent { get; set; }
        public List<MenuTree> Children { get; set; }
    }


}
