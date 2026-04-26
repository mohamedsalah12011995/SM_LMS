namespace RM.Users.Records
{
    public record GetEmployeeInformationRecord
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
