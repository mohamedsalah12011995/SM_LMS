#nullable disable

using RM.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("JobAdvertisement", Schema ="Job")]
    public  class JobAdvertisement
    {
        public JobAdvertisement()
        {
            JobCareers = new List<JobCareer>();
        }
       
        public int Id { get; set; }
        public int? EntityId { get; set; }
        public int? ReferenceId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Code { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        [ForeignKey("DeletedByNavigation")]
        public int? DeletedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("UpdatedByNavigation")]
        public int? UpdatedBy { get; set; }
        public int? Type { get; set; }



        public bool? IsActive { get; set; }
        public DateTime? ActivatedDate { get; set; }

        [ForeignKey("ActivatedByNavigation")]
        public int? ActivatedBy { get; set; }

        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User ActivatedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }

        public virtual List<JobCareer> JobCareers { get; set; }

    }
}
