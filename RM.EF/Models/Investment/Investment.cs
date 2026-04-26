#nullable disable

namespace RM.Models
{
    public partial class Investment
    {
        public int Id { get; set; }
        public int? EntityId { get; set; }
        public int? ReferenceId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string OpportunityNo { get; set; }
        public int? OpportunityType { get; set; }
        public string OpportunityReference { get; set; }
        public DateTime? LastInquiryDate { get; set; }
        public DateTime? LastAdmissionDate { get; set; }
        public DateTime? OpenBidDate { get; set; }
        public DateTime? OpportunityDate { get; set; }
        public string OpportunityUrl { get; set; }
        public string Price { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public int? ActivatedBy { get; set; }

        public virtual User ActivatedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }

        public virtual InvestmentType OpportunityTypeNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
    }
}
