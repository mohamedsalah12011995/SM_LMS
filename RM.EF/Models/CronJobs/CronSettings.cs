using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Models
{
    [Table("CronSettings", Schema = "dbo")]

    public class CronSettings
    {
        public int Id { get; set; }
        public int? CronTypeId { get; set; }
        public string Emails { get; set; }

        [ForeignKey("Entity")]
        public int? EntityId { get; set; }
        public Entity Entity { get; set; }

        [ForeignKey("SubEntity")]
        public int? SubEntityId { get; set; }
        public Entity SubEntity { get; set; }

        [ForeignKey("Survey")]
        public int? SurveyId { get; set; }
        public Survey Survey { get; set; }

        public bool? IsActive { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }


        [ForeignKey("UpdatedByNavigation")]
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("Reference")]
        public int? ReferenceId { get; set; }

        public virtual Reference Reference { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }


    }
}
