using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.TemplateDtos
{
    public class FormValueDetails : BaseDto<FormValueDetails, Models.FormValueDetails>
    {

        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? _key { get; set; }
        [JsonIgnore]
        public int? _typeId { get; set; }
        public string Value { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public int? Order { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public decimal? NumericValue { get; set; }
        public bool? BooleanValue { get; set; }

        public string Property { get; set; }
        [JsonIgnore]
        public int? FormValueId { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string formValueId { set { FormValueId = Accessor.Set(value); } get { return Accessor.Get(FormValueId); } }
        public string Key { set { _key = Accessor.Set(value); } get { return Accessor.Get(_key); } }
        public string TypeId { set { _typeId = Accessor.Set(value); } get { return Accessor.Get(_typeId); } }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.FormValueDetails, FormValueDetails>()
                .Map(dest => dest._key, src => src.InputKey)
                .Map(dest => dest.Value, src => src.FormInput != null && src.FormInput.Type != (int)DynamicEnum.InputTypes.DropdownSingle ? src.InputValue : src.Description)
                .Map(dest => dest.Property, src => src.FormInput != null ? src.FormInput.Property.Trim() : null)
                .Map(dest => dest.Order, src => src.FormInput != null ? src.FormInput.Order : null)
                .Map(dest => dest._typeId, src => src.FormInput != null ? src.FormInput.Type : null)

                .Config;

        }



    }
}
