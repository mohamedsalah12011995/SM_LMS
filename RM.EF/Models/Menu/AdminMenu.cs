#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class AdminMenu
    {
        public AdminMenu()
        {
            InverseParent = new HashSet<AdminMenu>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Url { get; set; }
        public int? ParentId { get; set; }
        public int? ReferencesMajorId { get; set; }
        public int? ReferenceId { get; set; }
        public int? EntityId { get; set; }
        public int? MenuOrder { get; set; }
        public bool? IsSuperAdmin { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual AdminMenu Parent { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual ReferencesMajor ReferencesMajor { get; set; }
        public virtual ICollection<AdminMenu> InverseParent { get; set; }
    }
}
