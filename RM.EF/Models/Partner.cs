#nullable disable

using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class Partner
    {
        public int Id { get; set; }
        public int? ReferenceId { get; set; }
        public int? EntityId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string PartnershipTitleAr { get; set; }
        public string PartnershipTitleEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string AddressAr { get; set; }
        public string AddressEn { get; set; }
        public string RmdepartmentNameEn { get; set; }
        public string RmdepartmentNameAr { get; set; }
        public bool? ContractActive { get; set; }
        public DateTime? ContractDate { get; set; }
        public string IconUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public int? ActivatedBy { get; set; }

        public virtual User ActivatedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
    }
}
