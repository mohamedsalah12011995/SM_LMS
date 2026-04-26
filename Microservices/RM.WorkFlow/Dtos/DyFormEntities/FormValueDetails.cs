using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.WorkFlow.Dtos.DyFormEntities
{
    public class FormValueDetailsDto : BaseDto<FormValueDetailsDto, Models.FormValueDetails>
    {

        [JsonIgnore]
        public int? _id { get; set; }
        [JsonIgnore]
        public int? _key { get; set; }
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
        public int? _formValueId { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }
        public string formValueID { set { _formValueId = Accessor.Set(value); } get { return Accessor.Get(_formValueId); } }
        public string Key { set { _key = Accessor.Set(value); } get { return Accessor.Get(_key); } }
        [JsonIgnore]
        public int? _typeId { get; set; }
        public string TypeId { set { _typeId = Accessor.Set(value); } get { return Accessor.Get(_typeId); } }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.FormValueDetails, FormValueDetailsDto>()
                .Map(dest => dest._key, src => src.InputKey)
                .Map(dest => dest._formValueId, src => src.FormValueId)
                .Map(dest => dest.Value, src => src.FormInput != null && src.FormInput.Type != (int)DynamicFormEnums.InputTypes.DropdownSingle ? src.InputValue : src.Description)
                .Map(dest => dest.Property, src => src.FormInput != null ? src.FormInput.Property.Trim() : null)
                .Map(dest => dest.Order, src => src.FormInput != null ? src.FormInput.Order : null)
                .Map(dest => dest._typeId, src => src.FormInput != null ? src.FormInput.Type : null)

                .Config;

        }

    }
}
