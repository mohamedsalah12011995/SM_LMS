namespace RM.GovServices.Records
{
    public record GovServiceDeleteRecord
    {
        public string ID { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
