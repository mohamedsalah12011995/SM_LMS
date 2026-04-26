using RM.ContactUs.Dtos;
using RM.Core.Helpers;

namespace RM.ContactUs.Records
{
    public record AddActionRecord
    {
        public string contactId {  get; set; }
        public int? StatusId { get; set; }

        public string Note { get; set; }
        public string toReferenceId { get; set; }
        public List<ActionFiles> ActionFiles { get; set; }

    }
}
