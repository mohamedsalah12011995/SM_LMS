namespace RM.ContactUs.Records
{
    public record AddEvaluationRecord
    {
        public string contactUsId {  get; set; }
        public bool? IsPositive { get; set; }
        public string Note { get; set; }

    }
}
