namespace RM.Menu.Records
{
    public record GetMenuTreeRecord
    {
        public string referenceId { get; set; }
        public string rootParentId { get; set; }
        public string parentId { get; set; }
        public string Code { get; set; }

    }
}
