

namespace RM.Menu.Records
{
    public record ReOrderMenuTreeRecord
    {
        public string ID { get; set; }
        public List<MenuTreeRecord> menus { get; set; }

    }

    public record MenuTreeRecord
    {
        public string ParentId { get; set; }
        public int? MenuOrder { get; set; }
        public List<MenuTreeRecord> Children { get; set; }

    }
}
