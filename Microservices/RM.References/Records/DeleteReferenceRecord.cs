namespace RM.References.Records
{
    public record DeleteReferenceRecord
    {
        public string ID { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
