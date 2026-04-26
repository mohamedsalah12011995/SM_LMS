using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.TemplateDtos
{
    public class Theme : BaseDto<Theme, Models.Theme>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

    }
}
