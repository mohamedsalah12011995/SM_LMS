#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Models
{
    [Table("PermitsWorkSites", Schema = "Permits")]
    public class PermitWorkSite
    {
        public int Id { get; set; }

        [ForeignKey("PermitsRequestNavigation")]
        public int? PermitId { get; set; }

        [ForeignKey("WorksiteNavigation")]
        public int? WorksiteId { get; set; }

        public virtual PermitsRequest PermitsRequestNavigation { get; set; }
        public virtual MajorLookup WorksiteNavigation { get; set; }


    }
}
