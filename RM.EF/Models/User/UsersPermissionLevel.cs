using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RM.Models
{
    public partial class UsersPermissionLevel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? PermissionLevelId { get; set; }

        [NotMapped]
        public virtual PermissionLevel PermissionLevel { get; set; }
        [NotMapped]
        public virtual User User { get; set; }
    }
}
