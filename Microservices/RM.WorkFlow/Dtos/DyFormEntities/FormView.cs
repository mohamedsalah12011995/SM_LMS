using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.WorkFlow.Dtos.DyFormEntities
{
    public class FormView : BaseDto<FormView, Models.Form>
    {
        [JsonIgnore]
        public int? _id { get; set; }

        [JsonIgnore]
        public int? EntityId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        [JsonIgnore]
        public int? _referenceId { get; set; }
        [JsonIgnore]
        public int? _formTypeId { get; set; }

        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }

        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }
        public string referenceId { set { _referenceId = Accessor.Set(value); } get { return Accessor.Get(_referenceId); } }
        public string formTypeId { set { _formTypeId = Accessor.Set(value); } get { return Accessor.Get(_formTypeId); } }
        public bool? IsActive { get; set; }
        public List<ActionStepper> ActionStepper { get; set; } = new List<ActionStepper>();
        public List<int> JumpSteps { get; set; } = new List<int>();
        public string EntityUrl { get; set; }

        [JsonIgnore]
        public int? ThemeId { get; set; }
        public string themeId { set { ThemeId = Accessor.Set(value); } get { return Accessor.Get(ThemeId); } }

        [JsonIgnore]
        public int? FormValueId { get; set; }
        public string formValueId { set { FormValueId = Accessor.Set(value); } get { return Accessor.Get(FormValueId); } }

        public string ThemeName { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

        public static TypeAdapterConfig SelectConfig(string imagesGetPath, Models.EngineForms engineForm, List<ActionStepper> formSteps)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Form, FormView>()

                .Map(dest => dest.ThemeName, src => src.Theme != null ? src.Theme.Name : string.Empty)
                .Map(dest => dest.Icon, src => !string.IsNullOrEmpty(src.Icon) ? $"{imagesGetPath}/{src.Icon}" : $"{imagesGetPath}/noImage.png")
                .Map(dest => dest._id, src => engineForm.Form != null ? engineForm.Form.Id : 0)
                .Map(dest => dest.NameAr, src => engineForm.Form != null ? engineForm.Form.NameAr : string.Empty)
                .Map(dest => dest.NameEn, src => engineForm.Form != null ? engineForm.Form.NameEn : string.Empty)
                .Map(dest => dest._referenceId, src => engineForm.Form != null ? engineForm.Form.ReferenceId : null)
                .Map(dest => dest._formTypeId, src => engineForm.Form != null ? engineForm.Form.FormTypeId : null)
                .Map(dest => dest.IsActive, src => engineForm.Form != null ? engineForm.Form.IsActive : false)
                .Map(dest => dest.ActionStepper, src => formSteps)


                .Config;

        }

    }
    public class ActionStepper : BaseDto<ActionStepper, Models.EngineActionJobRole>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? EngineActionJobRoleId { get; set; }
        [JsonIgnore]
        public int? ReturnEngineActionJobRoleId { get; set; }

        [JsonIgnore]
        public int? RejectEngineActionJobRoleId { get; set; }
        [JsonIgnore]
        public int? JobRoleId { get; set; }
        [JsonIgnore]
        public int? ReturnActionId { get; set; }
        [JsonIgnore]
        public int? RejectActionId { get; set; }
        [JsonIgnore]
        public int? ClosedActionId { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string jobRoleId { set { JobRoleId = Accessor.Set(value); } get { return Accessor.Get(JobRoleId); } }
        public string returnActionId { set { ReturnActionId = Accessor.Set(value); } get { return Accessor.Get(ReturnActionId); } }
        public string rejectActionId { set { RejectActionId = Accessor.Set(value); } get { return Accessor.Get(RejectActionId); } }
        public string closedActionId { set { ClosedActionId = Accessor.Set(value); } get { return Accessor.Get(ClosedActionId); } }
        public string engineActionJobRoleId { set { EngineActionJobRoleId = Accessor.Set(value); } get { return Accessor.Get(EngineActionJobRoleId); } }
        public string returnEngineActionJobRoleId { set { ReturnEngineActionJobRoleId = Accessor.Set(value); } get { return Accessor.Get(ReturnEngineActionJobRoleId); } }
        public string rejectEngineActionJobRoleId { set { RejectEngineActionJobRoleId = Accessor.Set(value); } get { return Accessor.Get(RejectEngineActionJobRoleId); } }

        public bool? HasNote { get; set; }
        public bool? NoteIsRequired { get; set; }
        public int? StepNo { get; set; }
        public List<FormFieldDto> FormFields { get; set; } = new List<FormFieldDto>();
        public bool? IsTransferToReference { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.EngineActionJobRole, ActionStepper>()

                .Map(dest => dest.Id, src => src.ActionId)
                .Map(dest => dest.EngineActionJobRoleId, src => src.Id)
                .Map(dest => dest.NameAr, src => $" {src.ActionNavigation.NameAr} - {(src.JobRole != null ? src.JobRole.NameAr : string.Empty)}")
                .Map(dest => dest.NameEn, src => $" {src.ActionNavigation.NameEn} - {(src.JobRole != null ? src.JobRole.NameEn : string.Empty)}")
                .Map(dest => dest.ClosedActionId, src => src.CloseStep)

                .Config;





        }

    }

    public class FormFieldDto
    {
        public object Value { get; set; }
        public string ValueBase64 { get; set; }
        public string Url { get; set; }

        [JsonIgnore]
        public int? _key { get; set; }
        [JsonIgnore]
        public int? ActionId { get; set; }
        [JsonIgnore]
        public int? EngineActionJobRoleId { get; set; }

        [JsonIgnore]
        public int? EntityId { get; set; }

        public string LabelAr { get; set; }
        public string LabelEn { get; set; }
        public bool? IsUnique { get; set; }

        public bool? Required { get; set; }
        public string Validator { get; set; }
        public string ControlType { get; set; }
        [JsonIgnore]
        public int? Type { get; set; }
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
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public string type { set { Type = Accessor.Set(value); } get { return Accessor.Get(Type); } }

        public string Key { set { _key = Accessor.Set(value); } get { return Accessor.Get(_key); } }
        public string actionId { set { ActionId = Accessor.Set(value); } get { return Accessor.Get(ActionId); } }
        public string engineActionJobRoleId { set { EngineActionJobRoleId = Accessor.Set(value); } get { return Accessor.Get(EngineActionJobRoleId); } }
        public bool? Disable { get; set; }
        public bool? InputUseIntegration { get; set; }
        public string Length { get; set; }
        public bool? ShowInExport { get; set; }
        public List<FieldOptionsDto> Options { get; set; } = new List<FieldOptionsDto>();




    }


}
public class FieldOptionsDto
{
    [JsonIgnore]
    public int? _key { get; set; }
    public string Key { set { _key = Accessor.Set(value); } get { return Accessor.Get(_key); } }
    public string ValueAr { get; set; }
    public string ValueEn { get; set; }
    public string Code { get; set; }
}

public class UserActionJobRole
{
    [JsonIgnore]
    public int? EngineActionJobRoleId { get; set; }

    [JsonIgnore]
    public int? NextActionId { get; set; }
    [JsonIgnore]
    public int? ActionId { get; set; }

    [JsonIgnore]
    public int? JobRoleId { get; set; }
    [JsonIgnore]
    public int? ReturnActionId { get; set; }
    [JsonIgnore]
    public int? RejectActionId { get; set; }
    [JsonIgnore]
    public int? ClosedActionId { get; set; }

    public string actionId { set { ActionId = Accessor.Set(value); } get { return Accessor.Get(ActionId); } }
    public string engineActionJobRoleId { set { EngineActionJobRoleId = Accessor.Set(value); } get { return Accessor.Get(EngineActionJobRoleId); } }
    public string nextActionId { set { NextActionId = Accessor.Set(value); } get { return Accessor.Get(NextActionId); } }
    public string returnActionId { set { ReturnActionId = Accessor.Set(value); } get { return Accessor.Get(ReturnActionId); } }
    public string rejectActionId { set { RejectActionId = Accessor.Set(value); } get { return Accessor.Get(RejectActionId); } }
    public string closedActionId { set { ClosedActionId = Accessor.Set(value); } get { return Accessor.Get(ClosedActionId); } }
    public string jobRoleId { set { JobRoleId = Accessor.Set(value); } get { return Accessor.Get(JobRoleId); } }
    public int? StepNo { get; set; }
    public string ActionNameAr { get; set; }
    public string ActionNameEn { get; set; }


}

