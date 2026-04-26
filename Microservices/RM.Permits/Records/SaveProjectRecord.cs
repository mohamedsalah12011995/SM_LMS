
namespace RM.Permits.Records
{
    public record SaveProjectRecord
    {
        public string ID { get; set; }
        public string entityId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DetailsAr { get; set; }
        public string DetailsEn { get; set; }
        public List<FlowStepperProjectRecord> FlowStepperProject { get; set; }

    }

    public record FlowStepperProjectRecord
    {
        public string stepId {  get; set; }
        public int? OrderStep { get; set; }
    }
}
