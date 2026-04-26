namespace RM.Exams.Records
{
    public record SaveExamRecord
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
        public int? DistributionGradeMethod { get; set; }
    }
}
