#nullable disable

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("IdeaComments", Schema = "Innovation")]
    public  class IdeaComment
    {
        public int Id { get; set; }
        public string CommenterName { get; set; }
        public string Text { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public bool? IsApproved { get; set; }

        [ForeignKey("ApprovedByNavigation")]
        public int? ApprovedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public string ReplyText { get; set; }
        public DateTime? ReplyDate { get; set; }

        [ForeignKey("RepliedByNavigation")]

        public int? RepliedBy { get; set; }

        public string Email { get; set; }
        public bool? IsAgreeTerms { get; set; }

        [ForeignKey("Idea")]
        public int? IdeaId { get; set; }

        public virtual User ApprovedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User RepliedByNavigation { get; set; }
        public virtual Idea  Idea { get; set; }
    }
}
