#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models.Competitions
{
    public partial class TeamMember
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string IdCard { get; set; }
        public string Phone { get; set; }
        public int? CompetitorId { get; set; }

        public virtual Competitor Competitor { get; set; }
    }
}
