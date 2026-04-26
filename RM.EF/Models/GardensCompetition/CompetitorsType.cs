using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models.Competitions
{
    public partial class CompetitorsType
    {
        public CompetitorsType()
        {
            Competitors = new HashSet<Competitor>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<Competitor> Competitors { get; set; }
    }
}
