using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using RM.DynamicForms.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.TemplateDtos
{
    public class Form : BaseDto<Form, Models.Form>, IFilteration<Models.Form>, IFilterationForClone<Models.Form>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        public string EntityUrl { get; set; }

        [JsonIgnore]
        public int? ReferenceId { get; set; }

        [JsonIgnore]
        public int? CreatedBy { get; set; }

        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        [JsonIgnore]
        public int? FormTypeId { get; set; }

        [JsonIgnore]
        public int? WorkFlowEngineId { get; set; }

        [JsonIgnore]
        public int? ThemeId { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get(UpdatedBy); } }
        public string formTypeId { set { FormTypeId = Accessor.Set(value); } get { return Accessor.Get(FormTypeId); } }


        public string themeId { set { ThemeId = Accessor.Set(value); } get { return Accessor.Get(ThemeId); } }

        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public string workFlowEngineId { set { WorkFlowEngineId = Accessor.Set(value); } get { return Accessor.Get(WorkFlowEngineId); } }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsViewStatistic { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }

        public string ReferenceNameAr { get; set; }
        public string ReferenceNameEn { get; set; }
        public string FormTypeAr { get; set; }
        public string FormTypeEn { get; set; }
        public string StatusStringAr { get; set; }
        public string StatusStringEn { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string IconBase64 { get; set; }
        public string ThemeName { get; set; }
        public bool? UseIntegration { get; set; }
        public bool? NonEditableForm { get; set; }
        public bool? CheckPersonalData { get; set; }
        public bool? CheckApplicationNo { get; set; }
        public List<FormValue> FormValue { get; set; } = new List<FormValue>();
        public List<FormInput> FormInputs { get; set; } = new List<FormInput>();
        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.Form> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Form>(true);

            if (ReferenceId is not null)
                filter.And(u => u.ReferenceId == ReferenceId);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (!string.IsNullOrEmpty(NameAr))
                filter.And(u => u.NameAr.Contains(NameAr));

            if (!string.IsNullOrEmpty(NameEn))
                filter.And(u => u.NameEn.Contains(NameEn));

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;
        }

        public ExpressionStarter<Models.Form> FilterationForClone()
        {
            var filter = PredicateBuilder.New<Models.Form>(true);

            if (ReferenceId is not null)
                filter.And(u => u.ReferenceId != ReferenceId);

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;
        }


        public static TypeAdapterConfig SelectConfig(string imagesGetPath, JsonSerializerOptions jsonOptions)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Form, Form>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.StatusStringAr, src => src.IsActive == true ? "فعال" : "غير مفعل")
                .Map(dest => dest.StatusStringEn, src => src.IsActive == true ? "Active" : "DeActivate")
                .Map(dest => dest.ReferenceNameAr, src => src.Reference != null ? src.Reference.NameAr : string.Empty)
                .Map(dest => dest.ReferenceNameEn, src => src.Reference != null ? src.Reference.NameEn : string.Empty)
                .Map(dest => dest.FormTypeAr, src => src.FormType != null ? src.FormType.TypeAr : string.Empty)
                .Map(dest => dest.FormTypeEn, src => src.FormType != null ? src.FormType.TypeEn : string.Empty)

                .Map(dest => dest.ThemeName, src => src.Theme != null ? src.Theme.Name : string.Empty)
                .Map(dest => dest.Icon, src => !string.IsNullOrEmpty(src.Icon) ? $"{imagesGetPath}/{src.Icon}" : $"{imagesGetPath}/noImage.png")
                .Map(dest => dest.FormInputs, src => src.FormInputs.Any() ? src.FormInputs.Where(c => c.IsDeleted == false).Adapt<List<FormInput>>(FormInput.SelectConfig(jsonOptions)) : new List<FormInput>())

                .Config;

        }
        public TypeAdapterConfig AddConfig(int? userId, string IconBase64, string imagesSavePath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Form, Models.Form>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                 .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.ThemeId, src => src.ThemeId)
                .Map(dest => dest.Icon, !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(IconBase64)
                            ? Images.SaveSingleImageOnServer(IconBase64, null, imagesSavePath, false) : null)

                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId, string IconBase64, string imagesSavePath, Models.Form form)
        {
            return new TypeAdapterConfig()
                .NewConfig<Form, Models.Form>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Map(dest => dest.ThemeId, src => src.ThemeId)
                .Map(dest => dest.IsActive, src => src.IsActive.HasValue ? src.IsActive.Value : form.IsActive)
                .Map(dest => dest.Icon, !string.IsNullOrEmpty(IconBase64) ? !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(IconBase64)
                            ? Images.SaveSingleImageOnServer(IconBase64, null, imagesSavePath, false) : null : form.Icon)

                .Config;
        }

        public static TypeAdapterConfig ConfigFormToGetDataList(string imagesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Form, Form>()

                .Map(dest => dest.ThemeName, src => src.Theme != null ? src.Theme.Name : string.Empty)
                .Map(dest => dest.Icon, src => !string.IsNullOrEmpty(src.Icon) ? $"{imagesGetPath}/{src.Icon}" : $"{imagesGetPath}/noImage.png")

                .Config;

        }




    }
}
