using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class Role
    {
        public Role()
        {
            RolesPermissionLevels = new HashSet<RolesPermissionLevel>();
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public int? ReferenceId { get; set; }

        public virtual Reference Reference { get; set; }
        public virtual ICollection<RolesPermissionLevel> RolesPermissionLevels { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
