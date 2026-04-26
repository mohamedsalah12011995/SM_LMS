using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.InitiativesProjects.Dtos
{
    public class InitiativesProjectsBeneficiaries:BaseDto<InitiativesProjectsBeneficiaries,Models.InitiativesProjectsBeneficiary>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        [JsonIgnore]
        public int? InitiativesProjectId { get; set; }
        public string initiativesProjectId { set { InitiativesProjectId = Accessor.Set(value); } get { return Accessor.Get<int?>(InitiativesProjectId); } }
        [JsonIgnore]
        public int? BeneficiaryId { get; set; }
        public string beneficiaryId { set { BeneficiaryId = Accessor.Set(value); } get { return Accessor.Get<int?>(BeneficiaryId); } }
    }
}
