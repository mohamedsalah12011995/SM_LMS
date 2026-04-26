namespace RM.Investments.Records
{
    public record SaveInvestmentRecord
    {
        public string ID { get; set; }
        public string entityId { get; set; }
        public string referenceId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string OpportunityUrl { get; set; }
        public DateTime? LastAdmissionDate { get; set; }
        public DateTime? LastInquiryDate { get; set; }
        public DateTime? OpenBidDate { get; set; }
        public DateTime? OpportunityDate { get; set; }
        public string opportunityType { get; set; }
        public string Price { get; set; }
        public string OpportunityReference { get; set; }
        public string OpportunityNo { get; set; }

    }
}
