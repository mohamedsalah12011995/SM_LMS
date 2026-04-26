using RM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.Competitions.Dtos
{
    public class Competitors
    {
        [JsonIgnore]
        public int? _id { get; set; }
        [JsonIgnore]
        public int? _typeId { get; set; }
        [JsonIgnore]
        public int? _cityId { get; set; }
        [JsonIgnore]
        public int? _candidatedBy { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get<int?>(_id); } }
        public string TypeId { set { _typeId = Accessor.Set(value); } get { return Accessor.Get<int?>(_typeId); } }
        public string CityId { set { _cityId = Accessor.Set(value); } get { return Accessor.Get<int?>(_cityId); } }
        public string CandidatedBy { set { _candidatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(_candidatedBy); } }
        public string FullName { get; set; }
        public string IdCard { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string TeamName { get; set; }
        public string UniversityName { get; set; }
        public string SpecializationName { get; set; }
        public string CommercialNumber { get; set; }
        public string TypeName { get; set; }
        public string CityName { get; set; }
        public string UniversityApprovalDoc { get; set; }
        public string UniversityApprovalDocBase64 { get; set; }
        public string ProfileDoc { get; set; }
        public string ProfileDocBase64 { get; set; }
        public string IsTeamText { get; set; }
        public string IsLecturerText { get; set; }
        public string IsCandidatedText { get; set; }
        public string RepresentsUniversityText { get; set; }
        public string IsCompleteAttachFileText { get; set; }

        public string Code { get; set; }
        public int? AcademicYear { get; set; }
        public int? YearsOfExperience { get; set; }
        public int? TeamMemberCount { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? CandidatedDate { get; set; }
        public bool? RepresentsUniversity { get; set; }
        public bool? IsTeam { get; set; }
        public bool? IsLecturer { get; set; }
        public bool? IsCandidated { get; set; }
        public bool? IsCompleteAttachFile { get; set; }


        public List<TeamMembers> TeamMembers { get; set; }
        public Attachments Attachments { get; set; }
    }
}
