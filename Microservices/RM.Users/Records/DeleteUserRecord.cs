namespace RM.Users.Records
{
    public record DeleteUserRecord
    {
        public string ID { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
