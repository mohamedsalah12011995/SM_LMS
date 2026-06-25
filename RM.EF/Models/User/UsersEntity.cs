#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class UsersEntity
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? EntityId { get; set; }

        public bool? Add { get; set; }
        public bool? Edit { get; set; }
        public bool? Delete { get; set; }
        public bool? List { get; set; }
        public bool? Activate { get; set; }
        public bool? Reports { get; set; }
        public bool? View { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual User User { get; set; }
    }
}
