namespace RM.Surveys.Records
{
    public record SaveQuestionRecord
    {
        public string ID { get; set; }
        public string GroupId { get; set; }
        public string TypeQuestionId { get; set; }
        public string SurveyID { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool? Mandatory { get; set; }
        public bool? VerticalAnswersDirection { get; set; }
        public bool? IsFiltration { get; set; }
        public bool? IsGlobal { get; set; }
        public int? SubQuestionOrder { get; set; }
        public int? GroupOrder { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public bool? IsNoteRequired { get; set; }
        public string ImageBase64 { get; set; }
        public bool? IsActive { get; set; }
        public List<QuestionDataSourceRecord> SurveyDataSources { get; set; } 

    }

    public record QuestionDataSourceRecord
    {
        public string ID { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string ImageBase64 { get; set; }
        public int Rate { get; set; }

    }
}
