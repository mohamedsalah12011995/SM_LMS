using RM.Core.Helpers;

namespace RM.Volunteers.Records
{
    public record GetVolunteersListRecord
    {
        public string referenceId { get; set; }
        public string ageId { get; set; }
        public string genderId { get; set; }
        public string districtId { get; set; }
        public string qualificationId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

    }
}
