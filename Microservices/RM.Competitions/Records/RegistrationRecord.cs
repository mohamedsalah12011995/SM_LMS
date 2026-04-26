using RM.Competitions.Dtos;

namespace RM.Competitions.Records
{
    public record RegistrationRecord
    {
        public string FullName { get; set; }
        public string TypeId { get; set; }
        public string IdCard { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public bool? IsLecturer { get; set; }
        public bool? RepresentsUniversity { get; set; }
        public bool? IsTeam { get; set; }
        public string UniversityName { get; set; }
        public string SpecializationName { get; set; }
        public string TeamName { get; set; }
        public int? AcademicYear { get; set; }
        public string CommercialNumber { get; set; }
        public int? YearsOfExperience { get; set; }
        public bool? IsCompleteAttachFile { get; set; }
        public string ProfileDocBase64 { get; set; }

        public string UniversityApprovalDocBase64 { get; set; }
        public List<TeamMembers> TeamMembers { get; set; }


    }
}
