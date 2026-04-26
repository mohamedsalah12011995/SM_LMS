namespace RM.Surveys.Records
{
    public record GetSurveyDetailsRecord
    {
        public string ID { get; set; }
        public string referenceID { get; set; }
        public bool? IsActive { get; set; }
    }
}
