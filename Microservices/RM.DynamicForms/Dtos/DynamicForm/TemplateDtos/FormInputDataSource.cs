using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.TemplateDtos
{
    public class FormInputDataSource
    {
        [JsonIgnore]
        public int? _id { get; set; }
        public int? _dataSourceId { get; set; }

        [JsonIgnore]
        public int? _formInputId { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }
        public string DataSourceId { set { _dataSourceId = Accessor.Set(value); } get { return Accessor.Get(_dataSourceId); } }
        public string FormInputId { set { _formInputId = Accessor.Set(value); } get { return Accessor.Get(_formInputId); } }

    }
}
