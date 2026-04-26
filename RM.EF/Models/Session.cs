using System;
using System.Collections.Generic;

#nullable disable

namespace RM.Models
{
    public partial class Session
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsAuthenticated { get; set; }
        public DateTime? LastOperation { get; set; }
        public int? SystemId { get; set; }

        public virtual User User { get; set; }
    }
}
