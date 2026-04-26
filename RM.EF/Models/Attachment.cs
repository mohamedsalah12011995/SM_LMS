using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class Attachment
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Url { get; set; }
        public string Extention { get; set; }
        public int? ReferenceId { get; set; }
        public int? EntityId { get; set; }
        public int? ItemId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual MobileApplication Item { get; set; }
        public virtual Multimedia ItemNavigation { get; set; }
        public virtual Reference Reference { get; set; }
    }
}
