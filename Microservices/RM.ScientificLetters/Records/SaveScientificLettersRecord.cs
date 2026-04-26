namespace RM.ScientificLetters.Records
{
    public record SaveScientificLettersRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string degreeId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        public string ResearcherNameAr { get; set; }
        public string ResearcherNameEn { get; set; }
        public string ResearchPlaceAr { get; set; }
        public string ResearchPlaceEn { get; set; }

        public string DetailsAr { get; set; }
        public string DetailsEn { get; set; }

        public string FileBase64 { get; set; }
        public string FileType { get; set; }

        public DateTime? PublishedOn { get; set; }

    }
}
