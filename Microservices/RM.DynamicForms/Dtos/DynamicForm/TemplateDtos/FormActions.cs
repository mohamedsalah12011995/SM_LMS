using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.TemplateDtos
{
    public class FormActions
    {
        public List<TemplateForms> Forms { get; set; } = new List<TemplateForms>();

        public bool? IsDeleted { get; set; }

    }

    public class TemplateForms
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public bool? IsActive { get; set; }
    }
}
