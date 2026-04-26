using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Models
{
    [Table("ScientificLetters", Schema = "ScientificLetters")]

    public class ScientificLetters
    {
        public int Id { get; set; }
        public int? ReferenceId { get; set; }
        public int? EntityId { get; set; }
        public string ResearcherNameAr { get; set; }
        public string ResearcherNameEn { get; set; }
        public string ResearchPlaceAr { get; set; }
        public string ResearchPlaceEn { get; set; }
        public int? DegreeId { get; set; }
        public string FileName { get; set; }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        public string DetailsAr { get; set; }
        public string DetailsEn { get; set; }

        public DateTime? PublishedOn { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public DateTime? ActivatedDate { get; set; }

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set;}
        public int? ActivatedBy { get; set;}
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual User ActivatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual MajorLookup Degree { get; set; }






    }
}
