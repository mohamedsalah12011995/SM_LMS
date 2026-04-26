namespace RM.WorkFlow.Records
{
    public record SaveWorkFlowActionsRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
