using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("EngineForms", Schema = "WorkFlow")]
    public class EngineForms
    {
        public int Id { get; set; }
        public int? FormId { get; set; }
        public int? WorkFlowEngineId { get; set; }
        public bool? IsDeleted { get; set; }

        public Form Form { get; set; }
        public Engine WorkFlowEngine { get; set; }

    }
}
