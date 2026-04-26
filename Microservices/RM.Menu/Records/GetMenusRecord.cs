namespace RM.Menu.Records
{
    public record GetMenusRecord
    {
        public string ID { get; set; }
        public string referenceId {  get; set; }
        public string entityId { get; set; }
        public string Code { get; set; }
        public bool IsCms { get; set; }
    }
}
