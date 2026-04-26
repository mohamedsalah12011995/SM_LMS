

#nullable disable

namespace RM.Models
{
    public partial class Menu
    {
        public Menu()
        {
            InverseParent = new HashSet<Menu>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string Code { get; set; }
        public string Url { get; set; }
        public int? ReferenceId { get; set; }
        public int? ParentId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool? IsHidden { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? MenuOrder { get; set; }
        public int? ArticleId { get; set; }
        public int? EntityId { get; set; }
        public int? ReferenceMajorId { get; set; }
        public int? TypeId { get; set; }
        public bool? IsFirstRoot { get; set; }
        public string FontIcon { get; set; }
        public string ImageIcon { get; set; }
        public string SvgIcon { get; set; }
        public bool? OpenBlankPage { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        public virtual Article Article { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Menu Parent { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual ReferencesMajor ReferenceMajor { get; set; }
        public virtual MenuType Type { get; set; }
        public virtual ICollection<Menu> InverseParent { get; set; }
    }
}
