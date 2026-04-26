using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.TemplateDtos
{
    public class FormInput : BaseDto<FormInput, Models.FormInput>
    {


        [JsonIgnore]
        public int? Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        [JsonIgnore]
        public int? Type { get; set; }
        [JsonIgnore]
        public int? FormId { get; set; }

        [JsonIgnore]
        public int? ActionId { get; set; }

        [JsonIgnore]
        public int? EngineActionJobRoleId { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Mandatory { get; set; }
        public bool? VerticalDataSourceDirection { get; set; }
        public bool? ViewInFullRow { get; set; }
        public int? Order { get; set; }
        public bool? HasDataSourceFromAPI { get; set; }
        public string DataSourceAPIRouting { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string type { set { Type = Accessor.Set(value); } get { return Accessor.Get(Type); } }
        public string formId { set { FormId = Accessor.Set(value); } get { return Accessor.Get(FormId); } }
        public string actionId { set { ActionId = Accessor.Set(value); } get { return Accessor.Get(ActionId); } }
        public string engineActionJobRoleId { set { EngineActionJobRoleId = Accessor.Set(value); } get { return Accessor.Get(EngineActionJobRoleId); } }

        public string InputType { get; set; }
        public string APIParameters { get; set; }
        public string OnChangeAPIMethodName { get; set; }
        public string OnChangeParamName { get; set; }
        [JsonIgnore]
        public int? OnChangeRefelectionInputKey { get; set; }
        public string onChangeRefelectionInputKey { set { OnChangeRefelectionInputKey = Accessor.Set(value); } get { return Accessor.Get(OnChangeRefelectionInputKey); } }

        public bool? ShowInMainListCP { get; set; }
        public bool? ShowInMainPortalPage { get; set; }
        public bool? ShowInAdvancedSearch { get; set; }

        public string Property { get; set; }
        public bool? IsUnique { get; set; }
        public string GroupId { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public bool? InputUseIntegration { get; set; }
        public string Length { get; set; }
        public bool? ShowInExport { get; set; }


        public FormsDataSource FormsDataSource { get; set; }
        public LookupParameterModel LookupParameter { get; set; }


        public TypeAdapterConfig AddConfig(int? formId, JsonSerializerOptions jsonOptions)
        {
            return new TypeAdapterConfig()
                .NewConfig<FormInput, Models.FormInput>().IgnoreNullValues(true)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.FormId, src => formId)
                .Map(dest => dest.APIParameters, src => src.HasDataSourceFromAPI == true && !src.LookupParameter.ArePropertiesIsNull() ?
                            JsonSerializer.Serialize(src.LookupParameter, jsonOptions) : null)



                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? formId, JsonSerializerOptions jsonOptions)
        {
            return new TypeAdapterConfig()
                .NewConfig<FormInput, Models.FormInput>().IgnoreNullValues(true)
                .Map(dest => dest.FormId, src => formId)
                .Map(dest => dest.APIParameters, src => src.HasDataSourceFromAPI == true && !src.LookupParameter.ArePropertiesIsNull() ?
                            JsonSerializer.Serialize(src.LookupParameter, jsonOptions) : null)
                .Config;
        }



        public static TypeAdapterConfig SelectConfig(JsonSerializerOptions jsonOptions)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.FormInput, FormInput>()

                .Map(dest => dest.InputType, src => src.InputsType != null ? src.InputsType.Type : string.Empty)
                .Map(dest => dest.ViewInFullRow, src => src.ViewInFullRow == null ? false : src.ViewInFullRow)
                .Map(dest => dest.LookupParameter, src => src.APIParameters != null ? JsonSerializer.Deserialize<LookupParameterModel>(src.APIParameters, jsonOptions) : null)
                .Map(dest => dest.FormsDataSource, src => src.FormInputDataSource.Any() ? src.FormInputDataSource.Adapt<List<FormsDataSource>>(FormsDataSource.SelectConfig(src)) : new List<FormsDataSource>())

                .Config;

        }



    }


}
