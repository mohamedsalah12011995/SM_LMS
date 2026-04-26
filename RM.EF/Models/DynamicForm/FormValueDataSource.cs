using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("FormValueDataSource", Schema = "DynamicForm")]

    public class FormValueDataSource
    {
        public int Id { get; set; }
        public int? FormValueId { get; set; }
        public int? FormDataSourceId { get; set; }

        [ForeignKey("FormInput")]
        public int? InputKey { get; set; }
        public FormValue FormValue { get; set; }
        public FormsDataSource FormDataSource { get; set; }
        public FormInput FormInput { get; set; }
    }
}
