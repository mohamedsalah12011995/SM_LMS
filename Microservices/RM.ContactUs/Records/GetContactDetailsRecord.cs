namespace RM.ContactUs.Records
{
    public record GetContactDetailsRecord
    {
        public string entityId { get; set; }
        public string Code { get; set; }
        public string UserId { get; set; }
    }
}
