
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System;
using System.Text.Json.Serialization;

namespace RM.InitiativesProjects.Dtos
{
    public class Beneficiaries:BaseDto<Beneficiaries,Models.Beneficiary>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public DateTime? CreatedDate { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
    }
}
