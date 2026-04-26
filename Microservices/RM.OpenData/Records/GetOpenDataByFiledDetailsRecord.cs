namespace RM.OpenData.Records
{
    public record GetOpenDataByFiledDetailsRecord
    {
        public string districtId { get; set; }
        public string typeId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }

    }
}
