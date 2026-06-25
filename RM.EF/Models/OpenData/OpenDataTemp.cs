#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Models
{
    [Table("OpenDataTemp", Schema = "OpenData")]

    public class OpenDataTemp
    {
        public int Id { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }

        public int? DistrictId { get; set; }
        public int? TypeId { get; set; }
        public double? Value { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        public int? ReferenceId { get; set; }
        public int? EntityId { get; set; }
        public bool? IsConfirm { get; set; }
        public bool? IsGregorian { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User ModifiedByNavigation { get; set; }
        public virtual MajorLookup District { get; set; }
        public virtual MajorLookup Type { get; set; }
    }
}
