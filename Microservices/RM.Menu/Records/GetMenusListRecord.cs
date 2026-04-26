namespace RM.Menu.Records
{
    public record GetMenusListRecord
    {
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string Code { get; set; }
    }
}
