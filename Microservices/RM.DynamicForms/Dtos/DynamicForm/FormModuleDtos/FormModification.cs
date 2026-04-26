using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.FormModuleDtos
{
    public class FormModification
    {
        public List<FormsValueModification> Forms { get; set; } = new List<FormsValueModification>();
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

    }

    public class FormsValueModification
    {
        [JsonIgnore]
        public int? FormValueId { get; set; }
        public string ID { set { FormValueId = Accessor.Set(value); } get { return Accessor.Get(FormValueId); } }

    }
}
