namespace RM.Partners.Records
{
    public record SavePartnersRecord
    {
        public string ID { get; set; }
        public string entityId { get; set; }
        public string referenceId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string PartnershipTitleAr { get; set; }
        public string PartnershipTitleEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string AddressAr { get; set; }
        public string AddressEn { get; set; }
        public string RmdepartmentNameEn { get; set; }
        public string RmdepartmentNameAr { get; set; }
        public bool? ContractActive { get; set; }
        public DateTime? ContractDate { get; set; }
        public string IconUrlBase64 { get; set; }

    }
}
