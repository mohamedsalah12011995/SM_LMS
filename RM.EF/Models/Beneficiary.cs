using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class Beneficiary
    {
        public Beneficiary()
        {
            InitiativesProjectsBeneficiaries = new HashSet<InitiativesProjectsBeneficiary>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual ICollection<InitiativesProjectsBeneficiary> InitiativesProjectsBeneficiaries { get; set; }
    }
}
