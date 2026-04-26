namespace RM.Jobs.Records
{
    public record SearchApplicationRecord
    {
        public string referenceId {  get; set; }
        public string Code { get; set; }
        public string IdCardNumber { get; set; }
    }
}
