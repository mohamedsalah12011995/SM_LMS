using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class LoginWay
    {
        public LoginWay()
        {
            ReferencesMajors = new HashSet<ReferencesMajor>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<ReferencesMajor> ReferencesMajors { get; set; }
    }
}
