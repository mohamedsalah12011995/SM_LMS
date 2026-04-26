using RM.Models;
using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class InteractionStatistic
    {
        public int Id { get; set; }
        public int? ReferenceId { get; set; }
        public int? ReferenceMajorId { get; set; }
        public int? EntityId { get; set; }
        public int? ItemId { get; set; }
        public int? InteractionStatisticsType { get; set; }
        public int? Value { get; set; }
        public int? IsHelpfulCount { get; set; }
        public int? NotHelpfulCount { get; set; }
        public string ItemUrl { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Entity Entity { get; set; }
        public virtual InteractionStatisticsType InteractionStatisticsTypeNavigation { get; set; }
        //public virtual News Item { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual ReferencesMajor ReferenceMajor { get; set; }
    }
}
