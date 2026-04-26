using RM.Core.Helpers;

namespace RM.Entities.Records
{
    public record GetEntitiesListRecord
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool? IsActive { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }

    }
}
