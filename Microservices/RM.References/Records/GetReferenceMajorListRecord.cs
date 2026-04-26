using RM.Core.Helpers;

namespace RM.References.Records
{
    public record GetReferenceMajorListRecord
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string CreatedPerson { get; set; }
        public string UpdatedPerson { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

    }
}
