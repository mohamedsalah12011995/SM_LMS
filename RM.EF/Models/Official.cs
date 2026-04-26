using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class Official
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public DateTime? PeriodTo { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public string CvUrl { get; set; }
        public string Email { get; set; }
        public string JobTitleAr { get; set; }
        public string JobTitleEn { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public int? EntityId { get; set; }
        public int? ReferenceId { get; set; }
        public string ThumbPic { get; set; }
        public string OriginalPic { get; set; }
        public int? OfficialOrder { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public int? ActivatedBy { get; set; }

        public virtual User ActivatedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
    }
}
