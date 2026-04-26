namespace RM.Multimedia.Records
{
    public record SaveAlbumRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public List<AttachmentsRecord> ListOfImages { get; set; }

    }

    public record AttachmentsRecord
    {
        public string ID { get; set; }
        public string NameAr { get; set; }
        public string Url { get; set; }

    }
}
