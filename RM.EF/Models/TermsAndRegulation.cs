

#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    public partial class TermsAndRegulation
    {
        public TermsAndRegulation()
        {
            InverseParent = new HashSet<TermsAndRegulation>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Url { get; set; }
        public int? ReferenceId { get; set; }
        public int? EntityId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public int? ParentId { get; set; }
        public bool? IsFinalRoot { get; set; }
        public bool? IsActive { get; set; }
    
        public int? ActivatedBy { get; set; }
        public DateTime? ActivatedDate { get; set; }

        public virtual User ActivatedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual TermsAndRegulation Parent { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<TermsAndRegulation> InverseParent { get; set; }
    }
}
