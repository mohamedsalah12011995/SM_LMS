using DocumentFormat.OpenXml.Wordprocessing;
using RM.Core.Helpers;
using RM.Core.Integrations;

namespace RM.Users.Records
{
    public record GetUsersListRecord
    {
        public string referenceId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string EmployeeId { get; set; }
        public string IdCardNumber { get; set; }
        public string loginWayId { get; set; }
        public bool? IsBlocked { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


    }
}
