using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class Article
    {
        public Article()
        {

            Menus = new HashSet<Menu>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? EntityId { get; set; }
        public int? ReferenceId { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public int? ActivatedBy { get; set; }
        public string FrontIdentity { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string MenuCode { get; set; }
        public bool? ShowBySearch { get; set; }

        public virtual User ActivatedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
    }
}
