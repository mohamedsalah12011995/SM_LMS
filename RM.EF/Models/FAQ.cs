#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class FAQ
    {

        public int Id { get; set; }
        public int? ReferenceId { get; set; }
        public int? EntityId { get; set; }
        public string QuestionAr { get; set; }
        public string QuestionEn { get; set; }
        public string AnswerAr { get; set; }
        public string AnswerEn { get; set; }
        public bool? IsDeleted { get; set; }

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Reference Reference { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
    }
}
