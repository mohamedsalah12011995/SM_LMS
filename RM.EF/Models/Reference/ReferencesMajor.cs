
#nullable disable

namespace RM.Models
{
    public partial class ReferencesMajor
    {
        public ReferencesMajor()
        {
            AdminMenus = new HashSet<AdminMenu>();
            ArticlesPublishes = new HashSet<ArticlesPublish>();
            Entities = new HashSet<Entity>();
            InteractionStatistics = new HashSet<InteractionStatistic>();
            Menus = new HashSet<Menu>();
            References = new HashSet<Reference>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? CreatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public int? LoginWayId { get; set; }
        public int? EntityId { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual LoginWay LoginWay { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<AdminMenu> AdminMenus { get; set; }
        public virtual ICollection<ArticlesPublish> ArticlesPublishes { get; set; }
        public virtual ICollection<Entity> Entities { get; set; }
        public virtual ICollection<InteractionStatistic> InteractionStatistics { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
        public virtual ICollection<Reference> References { get; set; }
    }
}
