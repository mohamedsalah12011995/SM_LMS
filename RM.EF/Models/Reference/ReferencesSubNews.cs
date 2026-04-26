using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class ReferencesSubNews
    {
        public int Id { get; set; }
        public int? NewsId { get; set; }
        public int? ReferenceSubId { get; set; }
    }
}
