using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace RM.Models
{
    [Table("MajorLookupsType", Schema = "dbo")]
    public class MajorLookupsType
    {
        public MajorLookupsType()
        {
            MajorLookups =new HashSet<MajorLookup>();
        }
       
        [Column("ID")]
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<MajorLookup> MajorLookups { get; set; }
    }
}
