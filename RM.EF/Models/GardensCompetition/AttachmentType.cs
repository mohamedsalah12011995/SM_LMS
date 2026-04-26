using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models.Competitions
{
    public partial class AttachmentType
    {
        public AttachmentType()
        {
            Attachments = new HashSet<Attachment>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? MinCount { get; set; }
        public int? MaxCount { get; set; }
        public string AcceptedExtention { get; set; }
        public string Description { get; set; }
        public bool? IsRequired { get; set; }

        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}
