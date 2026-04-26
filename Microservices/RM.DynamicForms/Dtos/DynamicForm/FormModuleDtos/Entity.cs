using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.FormModuleDtos
{
    public class Entity : BaseDto<Entity, Models.Entity>
    {

        [JsonIgnore]
        public int? Id { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string FrontIdentity { get; set; }

        public string CmsIdentity { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }



    }
}
