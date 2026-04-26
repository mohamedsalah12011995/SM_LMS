namespace RM.Volunteers.Records
{
    public record SaveVolunteersRecord
    {
        public string ID {  get; set; }
        public string entityId {  get; set; }
        public string referenceId {  get; set; }
        public string ageId { get; set; }
        public string genderId { get; set; }
        public string districtId { get; set; }
        public string qualificationId { get; set; }
        public string volunteerFieldId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public DateTime? Birthday { get; set; }

        public string Capcha { get; set; }
    }
}
