using RM.Core.Helpers;

namespace RM.OpenData.Records
{
    public record GetOpenDataListRecord
    {
        public string referenceId { get; set; }
        public string parentTypeId { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? FromModifiedDate { get; set; }
        public DateTime? ToModifiedDate { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? FromCreatedDate { get; set; }

        public DateTime? ToCreatedDate { get; set; }
        public string districtId { get; set; }
        public string typeId { get; set; }
        public int? Year { get; set; }
        public int? FromYear { get; set; }
        public int? ToYear { get; set; }

        public int? Month { get; set; }
        public int? FromMonth { get; set; }
        public int? ToMonth { get; set; }
        public double? Value { get; set; }
        public bool? IsGregorian { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

    }
}
