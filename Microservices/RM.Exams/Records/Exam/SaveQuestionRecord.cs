namespace RM.Exams.Records
{
    public record SaveQuestionRecord
    {
        public string ID { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool? Mandatory { get; set; }
        public bool? VerticalAnswersDirection { get; set; }
        public double? Mark { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string typeId { get; set; }
        public List<ExamDataSourceRecord> ExamDataSources { get; set; } = new List<ExamDataSourceRecord>();
    }
}
