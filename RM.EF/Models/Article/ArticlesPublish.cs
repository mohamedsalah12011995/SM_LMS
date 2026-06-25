#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class ArticlesPublish
    {
        public int Id { get; set; }
        public int? ArticleId { get; set; }
        public int? ReferenceId { get; set; }
        public int? MajorReferenceId { get; set; }
        public DateTime? PublishedDate { get; set; }
        public int? PublishedBy { get; set; }
        public bool? Removed { get; set; }

        public virtual ReferencesMajor MajorReference { get; set; }
        public virtual User PublishedByNavigation { get; set; }
        public virtual Reference Reference { get; set; }
    }
}
