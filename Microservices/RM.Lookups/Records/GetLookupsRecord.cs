namespace RM.Lookups.Records
{
    public record GetLookupsRecord
    {
        public string typeId {  get; set; }
        public string parentId { get; set; }
        public string referenceId { get; set; }
    }
}
