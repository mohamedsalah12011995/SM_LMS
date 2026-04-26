using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("Themes", Schema = "DynamicForm")]
    public class Theme
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Form> Forms { get; set; } = new List<Form>();


    }
}
