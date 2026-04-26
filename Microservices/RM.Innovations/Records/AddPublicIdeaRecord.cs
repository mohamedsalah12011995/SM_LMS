namespace RM.Innovations.Records
{
    public record AddPublicIdeaRecord
    {
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string Capcha { get; set; }
        public string AttachmentBase64 { get; set; }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string IdeaAddress { get; set; }
        public string IdeaDescription { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public bool? IdeaExist { get; set; }

    }
}
