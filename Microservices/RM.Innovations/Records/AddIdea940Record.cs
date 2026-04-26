
namespace RM.Innovations.Records
{
    public record AddIdea940Record
    {
        public string ID { get; set; }
        public string AttachmentBase64 { get; set; }
        public string IdeaAddress { get; set; }
        public string IdeaDescription { get; set; }
        public bool? IdeaExist { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public bool? NeedsBudget { get; set; }
        public int? NeedsPeriod { get; set; }
        public string category { get; set; }
        public string priority { get; set; }
        public string type { get; set; }
        public string toReference { get; set; }
        public string status { get; set; }
        public bool? Capability { get; set; }
        public bool? Feasibility { get; set; }
        public AddIdea940UserRecord User { get; set; }

    }

    public record AddIdea940UserRecord
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
