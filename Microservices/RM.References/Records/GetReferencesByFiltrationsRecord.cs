using RM.Core.Helpers;

namespace RM.References.Records
{
    public record GetReferencesByFiltrationsRecord
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string CreatedPerson { get; set; }
        public string UpdatedPerson { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string parentId { get; set; }
        public string referencesMajorId { get; set; }
        public bool? HasContent { get; set; }

        public List<int> SearchInReferences { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }
    }
}
