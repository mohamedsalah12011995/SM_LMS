namespace RM.ContactUs.Records
{
    public record SaveContactUsRecord
    {
        public string ID { get; set; }
        public string Capcha { get; set; }

        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string ideaId { get; set; }

        public string Name { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string FileUrlBase64 { get; set; }
        public bool? IsImageAttached { get; set; }
        public string regionReferenceId { get; set; }


    }
}
