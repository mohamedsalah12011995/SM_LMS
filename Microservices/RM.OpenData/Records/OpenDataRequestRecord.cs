namespace RM.OpenData.Records
{
    public record OpenDataRequestRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Details { get; set; }
        public string Address { get; set; }
        public string Capcha { get; set; }

    }
}
