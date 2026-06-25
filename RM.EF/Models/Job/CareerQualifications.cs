

#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models.Models
{
    [Table("CareerQualifications", Schema = "Job")]
    public class CareerQualifications
    {
        public int Id { get; set; }
        public int JobCareerId { get; set; }
        public int EduSpecializationId { get; set; }
        public int? SubSpecializationId { get; set; }
    }
}
