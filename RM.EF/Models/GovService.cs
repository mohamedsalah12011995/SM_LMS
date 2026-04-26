using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RM.Models
{
    public partial class GovService
    {
        public GovService()
        {
            InverseParent = new HashSet<GovService>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string SiteNameAr { get; set; }
        public string SiteNameEn { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public string AudienceEn { get; set; }
        public string AudienceAr { get; set; }
        public string ServiceChannelEn { get; set; }
        public string ServiceChannelAr { get; set; }
        public string RequirementsEn { get; set; }
        public string RequirementsAr { get; set; }
        public string StepsEn { get; set; }
        public string StepsAr { get; set; }
        public string ServiceUrl { get; set; }
        public int? ParentId { get; set; }
        public bool? IsRoot { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ReferenceId { get; set; }
        public bool? IsDeleted { get; set; }
        public int? EntityId { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsActive { get; set; }
        public int? ActivatedBy { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        [ForeignKey("LessAverage")]
        public int? LessAverageId { get; set; }
        public Recommendations LessAverage { get; set; }


        [ForeignKey("Average")]
        public int? AverageId { get; set; }
        public Recommendations Average { get; set; }


        [ForeignKey("AboveAverage")]
        public int? AboveAverageId { get; set; }
        public Recommendations AboveAverage { get; set; }
        public virtual User ActivatedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual GovService Parent { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<GovService> InverseParent { get; set; }
    }
}
