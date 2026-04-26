using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Models
{
    [Table("LogInformation", Schema = "OpenData")]
    public class LogInformation
    {
        public int Id { get; set; }
        public int? Type { get; set; }
        [ForeignKey("Reference")]
        public int? ReferenceId { get; set; }
        [ForeignKey("Entity")]
        public int? EntityId { get; set; }
        public int? ItemId { get; set; }
        public string Template { get; set; }
        public string FileExtension { get; set; }
        public int? Value { get; set; }

        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
    }
}
