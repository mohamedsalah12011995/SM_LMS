namespace RM.OpenData.Records
{
    public record SaveOpenDataTempRecord
    {
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string districtId { get; set; }
        public string typeId { get; set; }
        public bool? IsGregorian { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public double? Value { get; set; }
    }
}
