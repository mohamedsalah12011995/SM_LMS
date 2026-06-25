#nullable disable

using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("JobLookUp", Schema = "Job")]
    public class JobLookUp
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public int TypeId { get; set; }
        public int? ReferenceId { get; set; }
        public virtual Reference Reference { get; set; }

    }
}
