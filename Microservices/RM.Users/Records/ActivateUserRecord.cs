namespace RM.Users.Records
{
    public record ActivateUserRecord
    {
        public string ID { get; set; }
        public bool? IsBlocked { get; set; }
    }
}
