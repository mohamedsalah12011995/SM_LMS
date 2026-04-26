namespace RM.Users.Records
{
    public record CheckUserLoginFromActiveDirectoryRecord
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string desiredUserName { get; set; }
    }
}
