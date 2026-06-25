#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class Volunteer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public int? QualificationId { get; set; }
        public int? DistrictId { get; set; }
        public int? AgeId { get; set; }
        public int? GenderId { get; set; }
        public int? VolunteerFieldId { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string Description { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ReferenceId { get; set; }
        public bool? IsDeleted { get; set; }
        public int? EntityId { get; set; }

        public virtual MajorLookup Age { get; set; }
        public virtual MajorLookup District { get; set; }
        public virtual MajorLookup Gender { get; set; }
        public virtual MajorLookup Qualification { get; set; }
        public virtual MajorLookup VolunteerField { get; set; }
    }
}
