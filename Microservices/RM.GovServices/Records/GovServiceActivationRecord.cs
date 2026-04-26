namespace RM.GovServices.Records
{
    public record GovServiceActivationRecord
    {
        public string ID { get; set; }
        public bool? IsActive { get; set; }

    }
}
