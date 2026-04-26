namespace RM.Jobs.Records
{
    public record QueryApplicationRecord
    {
        public string referenceId {  get; set; }
        public string Code { get; set; }
        public string IdCardNumber { get; set; }
    }
}
