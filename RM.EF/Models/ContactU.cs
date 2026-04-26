using RM.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RM.Models
{
    [Table("ContactUs", Schema = "Contact")]
    public partial class ContactU
    {
        public ContactU()
        {
            Actions = new HashSet<Actions>();
            Feedbacks = new List<Feedback>();   
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ReferenceId { get; set; }
        public bool? IsDeleted { get; set; }
        public int? EntityId { get; set; }
        
        [ForeignKey("ItemEntity")]
        public int? ItemEntityId { get; set; }
        public int? ItemId { get; set; }
        public string FileUrl { get; set; }
        public string ComplainId { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public DateTime? LastStatusDate { get; set; }

        [ForeignKey("FirstStatus")]
        public int? FirstStatusId { get; set; }
        [ForeignKey("LastStatus")]
        public int? LastStatusId { get; set; }
        
        [ForeignKey("FirstAction")]
        public int? FirstActionId { get; set; }

        [ForeignKey("LastAction")]
        public int? LastActionId { get; set; }
       
        [ForeignKey("LastUserAction")]
        public int? LastActionUser { get; set; }
      
        [ForeignKey("LastReferenceAction")]
        public int? LastActionReference { get; set; }
        public bool? TransferFromManager { get; set; }
        public int? ProcessingTimesCount { get; set; }
       
        public int? IdeaId { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual User ModifiedByNavigation { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual Status FirstStatus { get; set; }
        public virtual Status LastStatus { get; set; }
        public virtual Actions FirstAction { get; set; }
        public virtual Actions LastAction { get; set; }
        public virtual User LastUserAction { get; set; }
        public virtual Reference LastReferenceAction { get; set; }
        public virtual ICollection<Actions> Actions { get; set; }
        public virtual List<Feedback> Feedbacks { get; set; }
        public virtual Entity ItemEntity { get; set; }

    }
}
