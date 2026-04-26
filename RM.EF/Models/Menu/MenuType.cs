using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class MenuType
    {
        public MenuType()
        {
            Menus = new HashSet<Menu>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }
    }
}
