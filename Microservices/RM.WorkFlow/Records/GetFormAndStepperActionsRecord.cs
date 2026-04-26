namespace RM.WorkFlow.Records
{
    public record GetFormAndStepperActionsRecord
    {
        public string ID { get; set; }
        public string entityId { get; set; }
        public string EntityUrl { get; set; }
        public string referenceId { get; set; }
        public string formValueId { get; set; }

    }
}
