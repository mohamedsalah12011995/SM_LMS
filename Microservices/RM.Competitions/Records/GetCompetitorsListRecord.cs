namespace RM.Competitions.Records
{
    public record GetCompetitorsListRecord
    {
        public string TypeId { get; set; }
        public bool? IsCandidated { get; set; }
        public bool? IsCompleteAttachFile { get; set; }
        public string IsCandidatedText { get; set; }
        public string IsCompleteAttachFileText { get; set; }
    }
}
