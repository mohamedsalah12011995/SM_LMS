#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class MobileApplication
    {
        public MobileApplication()
        {
            Attachments = new HashSet<Attachment>();
            QuestionsAnswers = new HashSet<QuestionsAnswer>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? ReferenceId { get; set; }
        public int? EntityId { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string ImageUrl { get; set; }
        public string ApplicationSize { get; set; }
        public string UserManualUrlAr { get; set; }
        public string UserManualUrlEn { get; set; }
        public string AndroidUrl { get; set; }
        public string IosUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public int? ActivatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        public virtual User ActivatedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<QuestionsAnswer> QuestionsAnswers { get; set; }
    }
}
