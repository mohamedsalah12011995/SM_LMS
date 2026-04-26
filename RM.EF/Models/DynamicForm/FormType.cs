
using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("FormType", Schema = "DynamicForm")]
    public class FormType
    {
        public int Id { get; set; }
        public string TypeAr { get; set; }
        public string TypeEn { get; set; }
        public virtual ICollection<Form> Forms { get; set; } = new List<Form>();

    }
}
