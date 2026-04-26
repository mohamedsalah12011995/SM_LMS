#nullable disable

namespace RM.Models
{
    public class InvestmentType
    {

        public InvestmentType()
        {
            Investments = new HashSet<Investment>();
        }

        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<Investment> Investments { get; set; }
    }
}
