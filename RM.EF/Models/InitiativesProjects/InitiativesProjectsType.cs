using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class InitiativesProjectsType
    {
        public InitiativesProjectsType()
        {
            InitiativesProjects = new HashSet<InitiativesProject>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        public virtual ICollection<InitiativesProject> InitiativesProjects { get; set; }
    }
}
