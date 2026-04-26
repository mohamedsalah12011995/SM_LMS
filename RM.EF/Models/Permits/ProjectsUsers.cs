
using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("ProjectsUsers", Schema = "Permits")]
    public class ProjectsUsers
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProjectId { get; set; }
        public bool? IsEmployee { get; set; }
        public virtual User User { get; set; }
        public virtual Project Project { get; set; }
    }
}
