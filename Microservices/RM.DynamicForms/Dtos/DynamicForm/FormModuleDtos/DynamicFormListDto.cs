using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.FormModuleDtos
{
    public class DynamicFormListDto : IFilteration<Models.FormValue>
    {
        [JsonIgnore]
        public int? EntityId { get; set; }

        [JsonIgnore]
        public int? FormId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public string formId { set { FormId = Accessor.Set(value); } get { return Accessor.Get(FormId); } }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string EntityUrl { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<FormListHeader> FormListHeader { get; set; } = new List<FormListHeader>();
        public List<object> ListFormValue { get; set; } = new List<object>();
        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.FormValue> Filteration()
        {
            var filter = PredicateBuilder.New<Models.FormValue>(true);

            if (EntityId is not null)
                filter.And(u => u.EntityId == EntityId);

            if (FormId is not null)
                filter.And(u => u.FormId == FormId);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (FromDate.HasValue)
                filter.And(u => u.CreatedDate.Value.Date >= FromDate.Value.Date);

            if (ToDate.HasValue)
                filter.And(u => u.CreatedDate.Value.Date <= ToDate.Value.Date);

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted == false);

            return filter;
        }


    }

    public class FormListHeader
    {
        [JsonIgnore]
        public int? _key { get; set; }
        public string Key { set { _key = Accessor.Set(value); } get { return Accessor.Get(_key); } }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Value { get; set; }
        public string Property { get; set; }
        public int? Order { get; set; }

        public static List<FormListHeader> GetStaticHeaderAttributes()
        {
            return new List<FormListHeader>
            {
                new FormListHeader { TitleAr="صاحب الانشاء", TitleEn="Created By"},
                new FormListHeader { TitleAr="صاحب التحديث", TitleEn="Updated By"},
                new FormListHeader { TitleAr="تاريخ الانشاء", TitleEn="Created Date"},
                new FormListHeader { TitleAr="تاريخ التحديث", TitleEn="Updated Date"},
                new FormListHeader { TitleAr=" الحالة", TitleEn="Status"},
            };
        }
    }

    public class ListFormValue : BaseDto<ListFormValue, Models.FormValue>
    {
        [JsonIgnore]
        public int? FormValueId { get; set; }

        public string ID { set { FormValueId = Accessor.Set(value); } get { return Accessor.Get(FormValueId); } }

        [JsonIgnore]
        public int? FormId { get; set; }

        public string formId { set { FormId = Accessor.Set(value); } get { return Accessor.Get(FormId); } }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }

        [JsonIgnore]
        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public object Value { get; set; }
        [JsonIgnore]
        public int? _key { get; set; }
        public string Key { set { _key = Accessor.Set(value); } get { return Accessor.Get(_key); } }

        [JsonIgnore]
        public int? CreatedBy { get; set; }

        [JsonIgnore]
        public int? UpdatedBy { get; set; }

        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get(UpdatedBy); } }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string StatusStringAr { get; set; }
        public string StatusStringEn { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public List<TemplateDtos.FormValueDetails> FormValueDetails { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.FormValue, ListFormValue>()
                .Map(dest => dest.FormValueId, src => src.Id)
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.StatusStringAr, src => src.IsActive == true ? "فعال" : "غير مفعل")
                .Map(dest => dest.StatusStringEn, src => src.IsActive == true ? "Active" : "DeActivate")
                .Map(dest => dest.FormValueDetails, src => src.FormValueDetails.Any() ? src.FormValueDetails.Where(c => c.FormInput.ShowInMainListCP == true && c.FormInput.IsDeleted == false).Adapt<List<TemplateDtos.FormValueDetails>>(TemplateDtos.FormValueDetails.SelectConfig()) : new List<TemplateDtos.FormValueDetails>())

                .Config;

        }
        public static TypeAdapterConfig SelectForExportConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.FormValue, ListFormValue>()
                .Map(dest => dest.FormValueId, src => src.Id)
                .Map(dest => dest.FormValueDetails, src => src.FormValueDetails.Any() ? src.FormValueDetails.Where(c => c.FormInput.ShowInExport == true && c.FormInput.IsDeleted == false).Adapt<List<TemplateDtos.FormValueDetails>>(TemplateDtos.FormValueDetails.SelectConfig()) : new List<TemplateDtos.FormValueDetails>())

                .Config;

        }


    }

    public class FormPropertyList
    {
        public Dictionary<string, dynamic> keyValuePairs = new Dictionary<string, dynamic>();
    }

}
