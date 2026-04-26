using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Users.Dtos;

namespace RM.Users.Records
{
    public record SaveUserRecord
    {
        public string ID { get; set; }
        public string Name {  get; set; }
        public string Email {  get; set; }
        public string roleId {  get; set; }
        public string referenceId {  get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmployeeId { get; set; }
        public string DomainUser { get; set; }
        public string IdCardNumber { get; set; }
        public string loginWayId { get; set; }

        public string jobRole {  get; set; }
        public DateTime? BirthDate { get; set; }

        public List<EntitiesItemsRecord> Entities { get; set; } = new List<EntitiesItemsRecord>();
        public List<UserEntityReferenceRecord> UsersEntitiesReferences { get; set; } = new List<UserEntityReferenceRecord>();

    }
    public record EntitiesItemsRecord
    {
        public string ID {  get; set; }
        public string entityId {  get; set; }
        public bool? Add { get; set; }
        public bool? Edit { get; set; }
        public bool? Delete { get; set; }
        public bool? List { get; set; }
        public bool? Activate { get; set; }
        public bool? Reports { get; set; }
        public bool? View { get; set; }
    }
    public record UserEntityReferenceRecord
    {
        public string ID { get; set; }
        public string entityId { get; set; }
        public string referenceId { get; set; }
        public bool? Add { get; set; }
        public bool? Edit { get; set; }
        public bool? Delete { get; set; }
        public bool? List { get; set; }
        public bool? Activate { get; set; }
        public bool? Reports { get; set; }
        public bool? View { get; set; }
    }
}
