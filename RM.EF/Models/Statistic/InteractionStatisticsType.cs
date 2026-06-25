#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class InteractionStatisticsType
    {
        public InteractionStatisticsType()
        {
            InteractionStatistics = new HashSet<InteractionStatistic>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<InteractionStatistic> InteractionStatistics { get; set; }
    }
}
