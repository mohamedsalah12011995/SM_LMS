namespace RM.DynamicForms.Records
{
    public record GetFormForViewRecord
    {

        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string EntityUrl { get; set; }

    }
}
