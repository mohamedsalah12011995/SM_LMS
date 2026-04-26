using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Menu.Dtos
{
    public class AdminMenu : BaseDto<AdminMenu, Models.AdminMenu>
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

        [JsonIgnore]
        public int? FormId { get; set; }
        public string DynamicFormUrl { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Url { get; set; }
        public string CmsIdentity { get; set; }

        [JsonPropertyName("id")]
        public string _id { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get(ParentId); } }
        public string referencesMajorId { set { ReferencesMajorId = Accessor.Set(value); } get { return Accessor.Get(ReferencesMajorId); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        [JsonPropertyName("entityId")]
        public string _entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public string userId { set { UserId = Accessor.Set(value); } get { return Accessor.Get(UserId); } }
        public string formId { set { FormId = Accessor.Set(value); } get { return Accessor.Get(FormId); } }

        public string EntityNameAr { get; set; }
        public string EntityNameEn { get; set; }
        public int? MenuOrder { get; set; }

        public AdminMenu Parent { get; set; }
        public List<AdminMenu> SubMenu { get; set; }


        public static TypeAdapterConfig SelectConfig(List<Models.AdminMenu> adminMenu, List<Models.FormsEntity> dynamicForms)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.AdminMenu, AdminMenu>()
                .Map(dest => dest.EntityId, src => src.Entity != null ? src.Entity.Id : 0)
                .Map(dest => dest.Url, src => src.Entity != null ? src.Entity.BackendIdentity : null)
                .Map(dest => dest.CmsIdentity, src => src.Entity != null ? src.Entity.CmsIdentity : null)
                .Map(dest => dest.SubMenu, src => adminMenu.FindAll(m => m.ParentId == src.Id).OrderBy(x => x.MenuOrder)
                .Adapt<List<AdminMenu>>())

                .Map(dest => dest.FormId, src => src.Entity != null &&
                  dynamicForms.Select(c => c.EntityId).Contains(src.EntityId.Value) ?
                  dynamicForms.FirstOrDefault(c => c.EntityId == src.EntityId.Value).FormId : null)

               .Map(dest => dest.DynamicFormUrl, src => src.Entity != null &&
                 dynamicForms.Select(c => c.EntityId).Contains(src.EntityId.Value) ?
                 dynamicForms.FirstOrDefault(c => c.EntityId == src.EntityId.Value).Url : null)
                .Config;



        }


        public static TypeAdapterConfig SelectConfigMenuByRefernceId(List<Models.AdminMenu> menusByReference)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.AdminMenu, AdminMenu>()
                .Map(dest => dest.EntityNameAr, src => src.Entity != null ? src.Entity.NameAr : null)
                .Map(dest => dest.EntityNameEn, src => src.Entity != null ? src.Entity.NameAr : null)

                .Map(dest => dest.SubMenu, src => menusByReference.FindAll(m => m.ParentId == src.Id)
                .Adapt<List<AdminMenu>>())
                .Config;

        }



    }





}
