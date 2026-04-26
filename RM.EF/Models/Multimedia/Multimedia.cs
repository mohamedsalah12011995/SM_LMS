#nullable disable

namespace RM.Models
{
    public class Multimedia
    {
        public Multimedia()
        {
            Attachments = new HashSet<Attachment>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ImageUrl { get; set; }
        public int? ReferenceId { get; set; }
        public int? EntityId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string Url { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public int? ActivatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }

        public virtual User ActivatedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}
