using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public  class Rate
    {
        public int Id { get; set; }
        public int? EntityId { get; set; }
        public int? ReferenceId { get; set; }
        public int? ItemId { get; set; }
        public int? CreatedBy { get; set; }
        public int? Value { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ItemUrl { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
    }
}
