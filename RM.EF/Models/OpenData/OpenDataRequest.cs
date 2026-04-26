using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Models
{
    [Table("OpenDataRequests", Schema = "OpenData")]

    public class OpenDataRequest
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Details { get; set; }
        [ForeignKey("Reference")]
        public int? ReferenceId { get; set; }
        [ForeignKey("Entity")]
        public int? EntityId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        [ForeignKey("ModifiedByNavigation")]
        public int? ModifiedBy { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User ModifiedByNavigation { get; set; }

    }
}
