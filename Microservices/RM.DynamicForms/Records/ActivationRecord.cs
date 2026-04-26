namespace RM.DynamicForms.Records
{
    public record ActivationFormValueRecord
    {
        public string ID { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }


    }
}
