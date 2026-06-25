#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class IdeasCompetentAuthority
    {
        public int Id { get; set; }
        public int? ReferenceId { get; set; }

        public virtual Reference Reference { get; set; }
    }
}
