using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class IdeaAction
    {
        public int Id { get; set; }
        public int? IdeaId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? Type { get; set; }
        public string Note { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual Idea Idea { get; set; }
        public virtual MajorLookup TypeNavigation { get; set; }
    }
}
