namespace RM.Exams.Records
{
    public record SaveExamAllRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool? IsActive { get; set; }
        public int? Duration { get; set; }
        public int? TotalMark { get; set; }
        public int? SuccessMark { get; set; }
        public string certificateId { get; set; }

        public int? DistributionGradeMethod { get; set; }
        public List<ExamQuestionRecord> ExamQuesions { get; set; } = new List<ExamQuestionRecord>();
    }

    public record ExamQuestionRecord
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
    public record ExamDataSourceRecord
    {
        public string ID { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool? IsCorrect { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
