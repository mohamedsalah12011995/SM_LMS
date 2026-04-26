using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.TemplateDtos
{
    public class FormValue
    {
        [JsonIgnore]
        public int? _id { get; set; }
        [JsonIgnore]
        public int? _entityId { get; set; }

        [JsonIgnore]
        public int? _formId { get; set; }

        [JsonIgnore]
        public int? _createdBy { get; set; }

        [JsonIgnore]
        public int? _updatedBy { get; set; }

        public string Id { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }
        public string FormId { set { _formId = Accessor.Set(value); } get { return Accessor.Get(_formId); } }
        public string EntityId { set { _entityId = Accessor.Set(value); } get { return Accessor.Get(_entityId); } }
        public string CreatedBy { set { _createdBy = Accessor.Set(value); } get { return Accessor.Get(_createdBy); } }
        public string UpdatedBy { set { _updatedBy = Accessor.Set(value); } get { return Accessor.Get(_updatedBy); } }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }
        public List<FormField> FormFields { get; set; } = new List<FormField>();
        public List<FormValueDetails> FormValueDetails { get; set; } = new List<FormValueDetails>();
    }


}
