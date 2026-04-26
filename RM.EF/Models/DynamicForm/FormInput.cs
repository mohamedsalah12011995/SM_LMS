using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("FormInputs", Schema = "DynamicForm")]
    public class FormInput
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? Type { get; set; }
        public int? FormId { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? Mandatory { get; set; }
        public bool? VerticalDataSourceDirection { get; set; }
        public bool? ViewInFullRow { get; set; }
        public bool? HasDataSourceFromAPI { get; set; }
        public string DataSourceAPIRouting { get; set; }
        public int? Order { get; set; }
        public string APIParameters { get; set; }
        public string OnChangeAPIMethodName { get; set; }
        public string OnChangeParamName { get; set; }
        public int? OnChangeRefelectionInputKey { get; set; }
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
        public virtual Form Form { get; set; }

        public virtual InputsType InputsType { get; set; }
        public virtual ICollection<FormInputDataSource> FormInputDataSource { get; set; } = new List<FormInputDataSource>();
        public virtual ICollection<FormValueDetails> FormValueDetails { get; set; } = new List<FormValueDetails>();
        public virtual ICollection<FormValueDataSource> FormValueDataSource { get; set; } = new List<FormValueDataSource>();
        public virtual ICollection<FormInputsActions> FormInputsActionsNavigation { get; set; } = new List<FormInputsActions>();

    }
}
