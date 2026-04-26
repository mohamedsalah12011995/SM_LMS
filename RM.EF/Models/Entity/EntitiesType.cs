

#nullable disable

namespace RM.Models
{
    public partial class EntitiesType
    {
        public EntitiesType()
        {
            Entities = new HashSet<Entity>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<Entity> Entities { get; set; }
    }
}
