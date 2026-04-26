using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.TemplateDtos
{
    public class InputsType : BaseDto<InputsType, RM.Models.InputsType>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Type { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }




    }
}
