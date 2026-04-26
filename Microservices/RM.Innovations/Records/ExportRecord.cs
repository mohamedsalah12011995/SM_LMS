namespace RM.Innovations.Records
{
    public record ExportRecord
    {
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsShow { get; set; }
        public string priority { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public bool? Capability { get; set; }
        public bool? NeedsBudget { get; set; }
        public bool? Feasibility { get; set; }
    }
}
