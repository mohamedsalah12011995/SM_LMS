using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.TemplateDtos
{

    public class FormView : BaseDto<FormView, RM.Models.Form>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? FormTypeId { get; set; }

        [JsonIgnore]
        public int? ThemeId { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string formTypeId { set { FormTypeId = Accessor.Set(value); } get { return Accessor.Get(FormTypeId); } }
        public string themeId { set { ThemeId = Accessor.Set(value); } get { return Accessor.Get(ThemeId); } }

        public bool? IsActive { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string ThemeName { get; set; }
        public bool? UseIntegration { get; set; }
        public bool? NonEditableForm { get; set; }
        public bool? CheckPersonalData { get; set; }
        public bool? CheckApplicationNo { get; set; }
        public List<FormField> FormFields { get; set; } = new List<FormField>();




        public static TypeAdapterConfig SelectConfig(string imagesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<RM.Models.Form, FormView>()

                .Map(dest => dest.ThemeName, src => src.Theme != null ? src.Theme.Name : string.Empty)
                .Map(dest => dest.Icon, src => !string.IsNullOrEmpty(src.Icon) ? $"{imagesGetPath}/{src.Icon}" : $"{imagesGetPath}/noImage.png")
                .Map(dest => dest.FormFields, src => src.FormInputs.Any() ? src.FormInputs.Where(c => c.IsDeleted == false).OrderBy(c => c.Order).Adapt<List<FormField>>(FormField.SelectConfig()) : new List<FormField>())

                .Config;

        }




    }

    public class FormField
    {
        public object Value { get; set; }
        public string ValueBase64 { get; set; }
        public string Url { get; set; }

        [JsonIgnore]
        public int? _key { get; set; }
        [JsonIgnore]
        public int? _actionId { get; set; }
        [JsonIgnore]
        public int? _engineActionJobRoleId { get; set; }
        [JsonIgnore]
        public int? Type { get; set; }
        public string LabelAr { get; set; }
        public string LabelEn { get; set; }
        public bool? Required { get; set; }
        public string Validator { get; set; }
        public string ControlType { get; set; }
        public bool? VerticalDataSourceDirection { get; set; }
        public int? Order { get; set; }
        public string Description { get; set; }

        public bool? ViewInFullRow { get; set; }
        public bool? HasDataSourceFromAPI { get; set; }
        public string DataSourceAPIRouting { get; set; }
        public string APIParameters { get; set; }
        public bool? ShowInMainListCP { get; set; }
        public bool? ShowInMainPortalPage { get; set; }
        public bool? ShowInAdvancedSearch { get; set; }
        public string OnChangeAPIMethodName { get; set; }
        public string OnChangeParamName { get; set; }
        [JsonIgnore]
        public int? OnChangeRefelectionInputKey { get; set; }
        public string onChangeRefelectionInputKey { set { OnChangeRefelectionInputKey = Accessor.Set(value); } get { return Accessor.Get(OnChangeRefelectionInputKey); } }
        public string Property { get; set; }
        public string Key { set { _key = Accessor.Set(value); } get { return Accessor.Get(_key); } }
        public string ActionId { set { _actionId = Accessor.Set(value); } get { return Accessor.Get(_actionId); } }
        public string EngineActionJobRoleId { set { _engineActionJobRoleId = Accessor.Set(value); } get { return Accessor.Get(_engineActionJobRoleId); } }


        public string type { set { Type = Accessor.Set(value); } get { return Accessor.Get(Type); } }
        public bool? Disable { get; set; }
        public bool? IsUnique { get; set; }
        public string GroupId { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public bool? InputUseIntegration { get; set; }
        public string Length { get; set; }
        public bool? ShowInExport { get; set; }

        public List<FieldOptions> Options { get; set; } = new List<FieldOptions>();

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<RM.Models.FormInput, FormField>()
                .Map(dest => dest.ControlType, src => src.InputsType != null ? src.InputsType.Type : string.Empty)
                .Map(dest => dest.ViewInFullRow, src => src.ViewInFullRow == null ? false : src.ViewInFullRow)
                .Map(dest => dest._key, src => src.Id)
                .Map(dest => dest.LabelAr, src => src.NameAr)
                .Map(dest => dest.LabelEn, src => src.NameEn)
                .Map(dest => dest.Required, src => src.Mandatory)
                .Map(dest => dest.Options, src => src.FormInputDataSource.Any() ? src.FormInputDataSource.OrderBy(c => c.Id).Adapt<List<FieldOptions>>(FieldOptions.SelectConfig()) : new List<FieldOptions>())

                .Config;

        }

    }
    public class FieldOptions
    {
        [JsonIgnore]
        public int? _key { get; set; }
        public string Key { set { _key = Accessor.Set(value); } get { return Accessor.Get(_key); } }
        public string ValueAr { get; set; }
        public string ValueEn { get; set; }
        public string Code { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<RM.Models.FormInputDataSource, FieldOptions>()
                .Map(dest => dest._key, src => src.DataSourceId)
                .Map(dest => dest.ValueAr, src => src.DataSource != null ? src.DataSource.TextAr : string.Empty)
                .Map(dest => dest.ValueEn, src => src.DataSource != null ? src.DataSource.TextEn : string.Empty)


                .Config;

        }
    }
}
