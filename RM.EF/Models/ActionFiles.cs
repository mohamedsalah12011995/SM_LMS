using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("ActionFiles", Schema = "Contact")]
    public class ActionFiles
    {

        public int Id { get; set; }

        [ForeignKey("Action")]
        public int ActionId { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public virtual Actions Action { get; set; }
    }
}
