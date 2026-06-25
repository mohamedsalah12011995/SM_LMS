#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class Multimedium
    {
        public Multimedium()
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
        public bool? IsPublished { get; set; }
        public DateTime? PublishedDate { get; set; }
        public int? PublishedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual User PublishedByNavigation { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}
