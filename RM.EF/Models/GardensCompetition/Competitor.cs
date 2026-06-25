#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models.Competitions
{
    public partial class Competitor
    {
        public Competitor()
        {
            Attachments = new HashSet<Attachment>();
            TeamMembers = new HashSet<TeamMember>();
        }

        public int Id { get; set; }
        public int? TypeId { get; set; }
        public string FullName { get; set; }
        public string IdCard { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string TeamName { get; set; }
        public string UniversityName { get; set; }
        public string SpecializationName { get; set; }
        public int? AcademicYear { get; set; }
        public int? YearsOfExperience { get; set; }
        public string CommercialNumber { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public bool? RepresentsUniversity { get; set; }
        public string UniversityApprovalDoc { get; set; }
        public bool? IsTeam { get; set; }
        public bool? IsLecturer { get; set; }
        public string ProfileDoc { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsCandidated { get; set; }
        public int? CandidatedBy { get; set; }
        public DateTime? CandidatedDate { get; set; }
        public bool? IsCompleteAttachFile { get; set; }
        public DateTime? ComplateAttachDate { get; set; }

        public virtual CompetitorsType Type { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual ICollection<TeamMember> TeamMembers { get; set; }
    }
}
