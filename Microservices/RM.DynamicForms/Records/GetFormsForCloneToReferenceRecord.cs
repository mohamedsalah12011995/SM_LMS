using RM.Core.Helpers;

namespace RM.DynamicForms.Records
{
    public record GetFormsForCloneToReferenceRecord
    {
        public string referenceId { get; set; }
        public bool? IsActive { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

    }
}
