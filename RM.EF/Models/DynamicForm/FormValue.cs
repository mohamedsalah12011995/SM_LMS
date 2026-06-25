
#nullable disable

using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("FormValue", Schema = "DynamicForm")]
    public class FormValue
    {
        public int Id { get; set; }
        public int? EntityId { get; set; }
        public int? FormId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual Form Form { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }


        public virtual ICollection<FormValueDataSource> FormValueDataSource { get; set; } = new List<FormValueDataSource>();
        public virtual ICollection<FormValueDetails> FormValueDetails { get; set; } = new List<FormValueDetails>();
        public virtual ICollection<FormValuesActions> FormValuesActions { get; set; } = new List<FormValuesActions>();

    }
}
