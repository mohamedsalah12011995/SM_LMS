using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public  class News
    {
      
        public int Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string NewsSourceAr { get; set; }
        public string NewsSourceEn { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string NewsContentAr { get; set; }
        public string NewsContentEn { get; set; }
        public string ThumpPic { get; set; }
        public string OriginalPic { get; set; }
        public DateTime? NewsDate { get; set; }
        public string NewsDateH { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public int? ActivatedBy { get; set; }
        public int? ReferenceId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public string TagsAr { get; set; }
        public string TagsEn { get; set; }
        public int? EntityId { get; set; }
        public bool? ShowInHome { get; set; }

        public virtual User ActivatedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
      
    }
}
