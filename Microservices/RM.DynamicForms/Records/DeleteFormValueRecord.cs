namespace RM.DynamicForms.Records
{
    public record DeleteFormValueRecord
    {
        public string ID { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }


    }
}
