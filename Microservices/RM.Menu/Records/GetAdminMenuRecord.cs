namespace RM.Menu.Records
{
    public record GetAdminMenuRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string RootReferenceId { get; set; }
    }
}
