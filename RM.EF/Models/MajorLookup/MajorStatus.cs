
#nullable disable

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("MajorStatus", Schema = "Contact")]
    public class MajorStatus
    {
        public MajorStatus()
        {
            Status = new List<Status>();
        }
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public List<Status> Status { get; set; }
    }
}
