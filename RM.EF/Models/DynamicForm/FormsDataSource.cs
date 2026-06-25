#nullable disable

using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("FormsDataSource", Schema = "DynamicForm")]
    public class FormsDataSource
    {
        public FormsDataSource()
        {
            InverseParent = new List<FormsDataSource>();
        }
        public int Id { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public bool? IsDeleted { get; set; }
        public int? ParentId { get; set; }
        public virtual FormsDataSource Parent { get; set; }
        public virtual List<FormsDataSource> InverseParent { get; set; }

    }
}
