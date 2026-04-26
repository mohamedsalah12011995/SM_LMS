namespace RM.Innovations.Records
{
    public record SaveIdeaDetailRecord
    {
        public string ID { get; set; }
        public bool? IsShow { get; set; }
        public string category { get; set; }
        public string type { get; set; }
        public string priority { get; set; }
        public bool? Feasibility { get; set; }
        public bool? NeedsBudget { get; set; }
        public bool? Capability { get; set; }
        public int? NeedsPeriod { get; set; }
        public string status { get; set; }

    }
}
