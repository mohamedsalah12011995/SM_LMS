
using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("FormsEntity", Schema = "DynamicForm")]
    public class FormsEntity
    {
        public int Id { get; set; }
        public int? FormId { get; set; }
        public string Url { get; set; }
        public virtual Form Form { get; set; }
        public int? EntityId { get; set; }
        public virtual Entity Entity { get; set; }
    }
}
