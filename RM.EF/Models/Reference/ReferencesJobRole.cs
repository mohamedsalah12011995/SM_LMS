#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class ReferencesJobRole
    {
        public int Id { get; set; }
        public int? ReferenceId { get; set; }
        public int? JobRoleId { get; set; }

        public virtual JobRole JobRole { get; set; }
        public virtual Reference Reference { get; set; }
    }
}
