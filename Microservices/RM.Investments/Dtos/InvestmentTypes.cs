
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System;
using System.Text.Json.Serialization;

namespace RM.Investments.Dtos
{
    public class InvestmentTypes:BaseDto<InvestmentTypes,Models.InvestmentType>
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
        public DateTime? ModifiedDate { get; set; }
        [JsonIgnore]
        public int? ModifiedBy { get; set; }
        public string modifiedBy { set { ModifiedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ModifiedBy); } }

         

    }
}
