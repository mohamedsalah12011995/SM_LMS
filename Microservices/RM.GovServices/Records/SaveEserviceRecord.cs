namespace RM.GovServices.Records
{
    public record SaveEserviceRecord
    {
        public string ID {  get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string parentId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string RequirementsAr { get; set; }
        public string RequirementsEn { get; set; }
        public string AttachmentsAr { get; set; }
        public string AttachmentsEn { get; set; }
        public string ServiceLink { get; set; }
        public string CategoryAr { get; set; }
        public string CategoryEn { get; set; }

        public string ContentEn { get; set; }
        public string ContentAr { get; set; }

        public string ContactEn { get; set; }
        public string ContactAr { get; set; }

        public string DeliveryChannelAr { get; set; }
        public string DeliveryChannelEn { get; set; }
        public string FeesAr { get; set; }
        public string FeesEn { get; set; }
        public string TypeAr { get; set; }
        public string TypeEn { get; set; }
        public string PaymentChannelAr { get; set; }
        public string PaymentChannelEn { get; set; }
        public string ExecutionTime { get; set; }
        public string ExecutionTimeEn { get; set; }
        public string ProceduresAr { get; set; }
        public string ProceduresEn { get; set; }
        public string FeesInk { get; set; }
        public string Path { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
