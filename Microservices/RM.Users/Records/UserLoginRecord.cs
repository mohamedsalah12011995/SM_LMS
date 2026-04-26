namespace RM.Users.Records
{
    public record UserLoginRecord
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Otp { get; set; }
    }
}
