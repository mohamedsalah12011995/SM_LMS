using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("PermitsRequests", Schema = "Permits")]
    public class PermitsRequest
    {
        public PermitsRequest()
        {
            this.PermitActions = new List<PermitAction>();
            this.PermitWorkSites = new List<PermitWorkSite>();
        }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Nationality { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }

        public int? Gender { get; set; }
        public int? LifeStatusCode { get; set; }

        public string JobName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string ExpiryDate { get; set; }
        public string IdentityNumber { get; set; }
        public string IdentitySource { get; set; }

        public DateTime? PermitStartDate { get; set; }
        public DateTime? PermitEndDate { get; set; }
        public int? PermitDays { get; set; }
        public string IdentityPhoto { get; set; }
        public int? PermitState { get; set; }
        public string PersonalPhoto { get; set; }
        public string CarModel { get; set; }
        public string CarColor { get; set; }
        public string CarLitters { get; set; }
        public string CarNumbers { get; set; }
        public int PermitType { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string Notes { get; set; }
        public string CarType { get; set; }

        public int? EntityId { get; set; }

        [ForeignKey("DeliverReference")]
        public int? DeliverReferenceId { get; set; }

        [ForeignKey("UserReference")]
        public int? ReferenceId { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        [ForeignKey("DeletedByNavigation")]
        public int? DeletedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("UpdatedByNavigation")]
        public int? UpdatedBy { get; set; }

        public DateTime? ActivatedDate { get; set; }
        public bool? IsForigin { get; set; }
        public bool? IsCommitted { get; set; }


        [ForeignKey("ActivatedByNavigation")]
        public int? ActivatedBy { get; set; }
        public string Attachment1 { get; set; }
        public string Attachment2 { get; set; }
        [ForeignKey("CurrentStepNavigation")]
        public int? CurrentStep { get; set; }
        [ForeignKey("NextStepNavigation")]
        public int? NextStep { get; set; }

        public int? ProjectId { get; set; }
        public int? LastUserActionJobRole { get; set; }

        public virtual Entity Entity { get; set; }
        public virtual Reference DeliverReference { get; set; }
        public virtual Reference UserReference { get; set; }
        public virtual User ActivatedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual FlowStepper CurrentStepNavigation { get; set; }
        public virtual FlowStepper NextStepNavigation { get; set; }

        public virtual Project Project { get; set; }

        public virtual List<PermitAction> PermitActions { get; set; }
        public virtual List<PermitWorkSite> PermitWorkSites { get; set; }


    }
}
