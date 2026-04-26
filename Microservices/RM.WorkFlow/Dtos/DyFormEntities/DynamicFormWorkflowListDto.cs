using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.WorkFlow.Dtos.DyFormEntities
{
    public class DynamicFormWorkflowListDto
    {
        [JsonIgnore]
        public int? _entityId { get; set; }

        [JsonIgnore]
        public int? _formId { get; set; }
        public string EntityId { set { _entityId = Accessor.Set(value); } get { return Accessor.Get(_entityId); } }
        public string FormId { set { _formId = Accessor.Set(value); } get { return Accessor.Get(_formId); } }

        [JsonIgnore]
        public int? _referenceId { get; set; }
        public string ReferenceId { set { _referenceId = Accessor.Set(value); } get { return Accessor.Get(_referenceId); } }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string EntityUrl { get; set; }
        public List<FormListHeaderDto> FormListHeader { get; set; } = new List<FormListHeaderDto>();

        public List<object> ListFormValue { get; set; } = new List<object>();
        public ApplicationOperation.Pagination Pagination { get; set; }
        public HeaderUserActions HeaderUserActions { get; set; }


    }

    public class FormListHeaderDto
    {
        [JsonIgnore]
        public int? _key { get; set; }
        public string Key { set { _key = Accessor.Set(value); } get { return Accessor.Get(_key); } }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Value { get; set; }
        public string Property { get; set; }
        public int? Order { get; set; }

        public static List<FormListHeaderDto> GetStaticHeaderAttributes()
        {
            return new List<FormListHeaderDto>
            {
                new FormListHeaderDto { TitleAr="مسؤل الاجراء", TitleEn="Created By"},
                new FormListHeaderDto { TitleAr=" ملاحظة الاجراء", TitleEn="Notes"},
                new FormListHeaderDto { TitleAr=" حالة البلاغ", TitleEn="Report Status"},
                new FormListHeaderDto { TitleAr=" الحالة", TitleEn="Status"},
            };
        }
    }


}
