#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class JobRole
    {
        public JobRole()
        {
            ReferencesJobRoles = new HashSet<ReferencesJobRole>();
            EnginesActionsJobRole = new HashSet<EngineActionJobRole>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<ReferencesJobRole> ReferencesJobRoles { get; set; }
        public virtual ICollection<EngineActionJobRole> EnginesActionsJobRole { get; set; }
    }
}
