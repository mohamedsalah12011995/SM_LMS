namespace RM.Users.Records
{
    public record CompleteUserRegistrationRecord
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
