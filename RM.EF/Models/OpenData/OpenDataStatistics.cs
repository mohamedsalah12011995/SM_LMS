#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Models
{
    [Table("OpenDataStatistics", Schema = "OpenData")]

    public class OpenDataStatistics
    {
        public int Id { get; set; }
        public int? ReferenceId { get; set; }
        public int? StatisticType { get; set; }
        public int? TypeId { get; set; }

        public int? FromYear { get; set; }
        public int? ToYear { get; set; }
        public int? FromMonth { get; set; }
        public int? ToMonth { get; set; }
        public int? DistrictId { get; set; }
        public bool? IsGregorian { get; set; }

        public DateTime? CreatedDate { get; set; }
        public int? Count { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual MajorLookup District { get; set; }
        public virtual MajorLookup Type { get; set; }
    }
}
