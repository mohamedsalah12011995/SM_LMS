using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Permits.Dtos
{
    public class CompanyRefernce
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? UseId { get; set; }
        public string UserId { set { UseId = Accessor.Set(value); } get { return Accessor.Get<int?>(UseId); } }
        public string CompanyUserName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string AddressEn { get; set; }
        public string Region { get; set; }
        public string Mailbox { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string ManagerName { get; set; }
        public string RegistrationNo { get; set; }
        public string CompanyName { get; set; }
        public string ItemUrl { get; set; }
        public DateTime? EndDateRegistrationNo { get; set; }
        public string EndDateRegistrationNoString { get; set; }


    }
}
