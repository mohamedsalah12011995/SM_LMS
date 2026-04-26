

namespace RM.Models
{
    public class EntitiesLatestUpdate
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int EntityId { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
