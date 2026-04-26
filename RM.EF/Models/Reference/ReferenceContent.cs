

#nullable disable

namespace RM.Models
{
    public partial class ReferenceContent
    {
        public int Id { get; set; }
        public int? ReferenceId { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string Url { get; set; }
        public string ChiefNameEn { get; set; }
        public string ChiefName { get; set; }
        public string ChiefImage { get; set; }
        public string ChiefWordEn { get; set; }
        public string ChiefWord { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AddressEn { get; set; }
        public string Address { get; set; }
        public string Region { get; set; }
        public string Mailbox { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string ManagerName { get; set; }
        public string RegistrationNo { get; set; }
        public DateTime? EndDateRegistrationNo { get; set; }
        public int? EntityId { get; set; }

        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
    }
}
