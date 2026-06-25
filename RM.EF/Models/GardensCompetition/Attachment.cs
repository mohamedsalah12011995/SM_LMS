#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models.Competitions
{
    public partial class Attachment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int? TypeId { get; set; }
        public int? CompetitorId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Competitor Competitor { get; set; }
        public virtual AttachmentType Type { get; set; }
    }
}
