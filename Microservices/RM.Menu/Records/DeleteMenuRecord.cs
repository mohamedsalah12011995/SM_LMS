namespace RM.Menu.Records
{
    public record DeleteMenuRecord
    {
        public string ID { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
