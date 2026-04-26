using RM.Models;
using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public int? EntityId { get; set; }
        public int? ReferenceId { get; set; }
        public string Text { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public bool? IsApproved { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ReplyText { get; set; }
        public DateTime? ReplyDate { get; set; }
        public int? RepliedBy { get; set; }
        public int? ItemId { get; set; }
        public string ItemUrl { get; set; }
        public string CommenterName { get; set; }
        public string Email { get; set; }
        public bool? IsAgreeTerms { get; set; }

        public virtual User ApprovedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User RepliedByNavigation { get; set; }
    }
}
