using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class RolesPermissionLevel
    {
        public int Id { get; set; }
        public int? PermissionLevelId { get; set; }
        public int? RoleId { get; set; }

        public virtual PermissionLevel PermissionLevel { get; set; }
        public virtual Role Role { get; set; }
    }
}
