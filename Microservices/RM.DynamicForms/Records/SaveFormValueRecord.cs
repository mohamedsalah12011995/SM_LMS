namespace RM.DynamicForms.Records
{
    public record SaveFormValueRecord
    {
        public string Id { get; set; }
        public string FormId { get; set; }
        public string EntityId { get; set; }
        public List<FormFieldRecord> FormFields { get; set; } = new List<FormFieldRecord>();


    }

    public record FormFieldRecord
    {
        public string Key { get; set; }
        public string ControlType { get; set; }
        public object Value { get; set; }
        public string ValueBase64 { get; set; }
        public string Url { get; set; }
        public string type { get; set; }
        public int? Order { get; set; }
        public string Description { get; set; }
        public bool? IsUnique { get; set; }
        public bool? HasDataSourceFromAPI { get; set; }

    }


}
