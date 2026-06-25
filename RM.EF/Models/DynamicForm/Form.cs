#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("Forms", Schema = "DynamicForm")]
    public class Form
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [ForeignKey("Reference")]
        public int? ReferenceId { get; set; }

        public int? FormTypeId { get; set; }
        public int? ThemeId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public bool? IsViewStatistic { get; set; }

        public bool? UseIntegration { get; set; }
        public bool? NonEditableForm { get; set; }
        public bool? CheckPersonalData { get; set; }
        public bool? CheckApplicationNo { get; set; }

        public virtual Reference Reference { get; set; }
        public virtual FormType FormType { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual Theme Theme { get; set; }
        public virtual ICollection<FormValue> FormValue { get; set; } = new List<FormValue>();
        public virtual ICollection<FormInput> FormInputs { get; set; } = new List<FormInput>();

        public Form Clone(int? userId)
        {
            return new Form
            {
                NameAr = $" نسخة _ {this.NameAr}",
                NameEn = $" Copy _ {this.NameEn}",
                DescriptionAr = this.DescriptionAr,
                DescriptionEn = this.DescriptionEn,
                FormTypeId = this.FormTypeId,
                FormType = this.FormType,
                Url = this.Url,
                CreatedBy = userId,
                CreatedDate = DateTime.Now,
                UpdatedBy = userId,
                UpdatedDate = DateTime.Now,
                UseIntegration = this.UseIntegration,
                NonEditableForm = this.NonEditableForm,
                CheckApplicationNo = this.CheckApplicationNo,
                CheckPersonalData = this.CheckPersonalData,
                ThemeId = this.ThemeId,
                Icon = this.Icon,
                FormInputs = this.FormInputs,
                IsActive = true,
                IsDeleted = false
            };

        }
    }


}
