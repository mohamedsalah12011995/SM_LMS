using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.TemplateDtos
{
    public class MokeData
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public string Name { get; set; }
    }
}
