using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class InitiativesProjectsBeneficiary
    {
        public int Id { get; set; }
        public int? InitiativesProjectId { get; set; }
        public int? BeneficiaryId { get; set; }

        public virtual Beneficiary Beneficiary { get; set; }
        public virtual InitiativesProject InitiativesProject { get; set; }
    }
}
