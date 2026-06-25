#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class PermissionLevel
    {
        public PermissionLevel()
        {
            RolesPermissionLevels = new HashSet<RolesPermissionLevel>();
            UsersPermissionLevels = new HashSet<UsersPermissionLevel>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<RolesPermissionLevel> RolesPermissionLevels { get; set; }
        public virtual ICollection<UsersPermissionLevel> UsersPermissionLevels { get; set; }
    }
}
