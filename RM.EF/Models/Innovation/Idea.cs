#nullable disable

using RM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RM.Models
{
    [Table("Ideas", Schema = "Innovation")]
    public partial class Idea
    {
        public Idea()
        {
            IdeaActions = new HashSet<IdeaAction>();
            IdeaComments=new HashSet<IdeaComment>();
        }

        public int Id { get; set; }
        public long? Code { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string IdeaAddress { get; set; }
        public string IdeaDescription { get; set; }
        public bool? IdeaExist { get; set; }
        public bool? IsShow { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Attachment { get; set; }
        public DateTime? CreatedDate { get; set; }
        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        [ForeignKey("DeletedByNavigation")]
        public int? DeletedBy { get; set; }
        public bool? IsActive { get; set; }
        [ForeignKey("ActivatedByNavigation")]
        public int? ActivatedBy { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [ForeignKey("UpdatedByNavigation")]
        public int? UpdatedBy { get; set; }
        public int? Type { get; set; }
        public int? Category { get; set; }
        [ForeignKey("ToReferenceNavigation")]
        public int? ToReference { get; set; }
        [ForeignKey("ReferenceNavigation")]
        public int? ReferenceId { get; set; }
        public int? EntityId { get; set; }

        public int? Priority { get; set; }
        public int? Status { get; set; }
        public bool? Capability { get; set; }
        public bool? NeedsBudget { get; set; }
        public bool? Feasibility { get; set; }
        public int? NeedsPeriod { get; set; }
        public int? AgreeCount { get; set; } = 0;
        public int? DisAgreeCount { get; set; } = 0;

        [ForeignKey("LastAction")]
        public int? LastActionId { get; set; }
        public virtual IdeaAction LastAction { get; set; }


        public virtual User ActivatedByNavigation { get; set; }
        public virtual MajorLookup CategoryNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual MajorLookup PriorityNavigation { get; set; }
        public virtual MajorLookup StatusNavigation { get; set; }
        public virtual Reference ReferenceNavigation { get; set; }
        public virtual Reference ToReferenceNavigation { get; set; }
        public virtual MajorLookup TypeNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<IdeaAction> IdeaActions { get; set; }
        public virtual ICollection<IdeaComment> IdeaComments { get; set;}

    }
}
