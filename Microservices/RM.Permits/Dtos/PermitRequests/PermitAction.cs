using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Permits.PermitEnum;
using System.Text.Json.Serialization;

namespace RM.Permits.Dtos
{
    public class PermitAction : BaseDto<PermitAction, Models.PermitAction>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        [JsonIgnore]
        public int? StepId { get; set; }

        public string stepId { set { StepId = Accessor.Set(value); } get { return Accessor.Get<int?>(StepId); } }

        [JsonIgnore]
        public int? CreatedBy { get; set; }

        [JsonIgnore]

        public int? UpdatedBy { get; set; }

        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }

        public bool? IsPrinted { get; set; }
        [JsonIgnore]
        public int? PermitRequestId { get; set; }
        public string permitRequestId { set { PermitRequestId = Accessor.Set(value); } get { return Accessor.Get<int?>(PermitRequestId); } }
        public string PersonUpdatedBy { get; set; }
        public string PersonCreatedBy { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string Notes { get; set; }
        public int? Status { get; set; }
        public string StatusAr { get; set; }
        public string StatusEn { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.PermitAction, PermitAction>()
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.Value.ToString("yyyy-MM-dd"))

                .Map(dest => dest.StatusAr, src => src.Status == (int)PermitEnums.PermitRequestStatus.New ? " تصريح جديد"
                    : src.Status == (int)PermitEnums.PermitRequestStatus.Verified ? "قبول التصريح"
                    : src.Status == (int)PermitEnums.PermitRequestStatus.Rejected ? "رفض التصريح"
                    : src.Status == (int)PermitEnums.PermitRequestStatus.Canceld ? "الغاء التصريح" : string.Empty)

                 .Map(dest => dest.StatusEn, src => src.Status == (int)PermitEnums.PermitRequestStatus.New ? "New Permit "
                    : src.Status == (int)PermitEnums.PermitRequestStatus.Verified ? " Permit Verified "
                    : src.Status == (int)PermitEnums.PermitRequestStatus.Rejected ? "Permit Rejected"
                    : src.Status == (int)PermitEnums.PermitRequestStatus.Canceld ? "Permit Canceld " : string.Empty)

                 .Config;
        }


        public TypeAdapterConfig AddConfig(int userId, Models.PermitsRequest permitRequest)
        {
            return new TypeAdapterConfig()
                .NewConfig<PermitAction, Models.PermitAction>().IgnoreNullValues(true)
                .Map(dest => dest.IsPrinted, src => false)
                .Map(dest => dest.CreatedBy, src => permitRequest.CreatedBy)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => permitRequest.CreatedDate)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Map(dest => dest.PermitRequestId, src => permitRequest.Id)


                .Config;
        }



    }
}
