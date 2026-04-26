using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("FormInputDataSource", Schema = "DynamicForm")]
    public class FormInputDataSource
    {
        public int Id { get; set; }
        public int? DataSourceId { get; set; }
        public virtual FormsDataSource DataSource { get; set; }
        public int? FormInputId { get; set; }
        public virtual FormInput FormInput { get; set; }
    }
}
