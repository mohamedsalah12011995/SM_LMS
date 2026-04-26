namespace RM.OpenData.Records
{
    public record GetOpenDataSearchStatisticsRecord
    {
        public string referenceId { get; set; }
        public string statisticTypeId { get; set; }
        public string parentTypeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? FromCreatedDate { get; set; }
        public DateTime? ToCreatedDate { get; set; }
        public string districtId { get; set; }
        public string typeId { get; set; }

        public int? FromYear { get; set; }
        public int? ToYear { get; set; }

        public int? FromMonth { get; set; }
        public int? ToMonth { get; set; }
        public bool? IsGregorian { get; set; }

    }
}
