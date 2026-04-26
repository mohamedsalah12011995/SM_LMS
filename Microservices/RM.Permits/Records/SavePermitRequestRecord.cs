namespace RM.Permits.Records
{
    public record SavePermitRequestRecord
    {
        public string ID { get; set; }
        public string entityId { get; set; }
        public string referenceId { get; set; }
        public string projectId {  get; set; }
        public int? PermitType { get; set; }
        public string IdentityPhotoBase64 { get; set; }
        public string PersonalPhotoBase64 { get; set; }
        public string Attachment1Base64 { get; set; }
        public string Attachment2Base64 { get; set; }
        public string deliverReferenceId { get; set; }
        public bool? IsCommitted { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string GenderId { get; set; }
        public string LifeCode { get; set; }
        public string DateOfBirth { get; set; }
        public string IdentityNumber { get; set; }
        public string IdentitySource { get; set; }
        public string ExpiryDateString { get; set; }
        public bool? IsForigin { get; set; }
        public string JobName { get; set; }
        public string Nationality { get; set; }
        public string CarType { get; set; }
        public string CarModel { get; set; }
        public string CarLitters { get; set; }
        public string CarNumbers { get; set; }
        public string CarColor { get; set; }
        public int? PermitDays { get; set; }
        public DateTime? PermitStartDate { get; set; }
        public string ItemUrl { get; set; }

        public List<PermitWorksiteRecord> ListOfWorkSites { get; set; }

    }

    public record PermitWorksiteRecord
    {
        public string ID { get; set; }
    }
}
