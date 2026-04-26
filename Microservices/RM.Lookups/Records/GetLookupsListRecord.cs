using RM.Core.Helpers;

namespace RM.Lookups.Records
{
    public record GetLookupsListRecord
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool? IsActive { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

    }
}
