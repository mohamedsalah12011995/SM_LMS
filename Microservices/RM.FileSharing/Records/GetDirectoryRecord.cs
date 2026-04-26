namespace RM.FileSharing.Records
{
    public record GetDirectoryRecord
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string SearchWord { get; set; }

        public DateTime? FromModifiedDate { get; set; }
        public DateTime? ToModifiedDate { get; set; }
        public bool? ModifiedToday { get; set; }
        public bool? ModifiedWeek { get; set; }
        public bool? ModifiedMonth { get; set; }
        public bool? ParentFolder { get; set; }

    }
}
