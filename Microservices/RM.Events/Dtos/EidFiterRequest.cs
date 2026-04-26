
using DocumentFormat.OpenXml.Wordprocessing;
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;

namespace RM.Events.Dtos
{
    public class EidFiterRequest:BaseDto<EidFiterRequest,Models.EidFiterRequest>
    {
        public int? Id { get; set; }
        public int? DistrictId { get; set; }
        public int? RequesterType { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string districtId { set { DistrictId = Accessor.Set(value); } get { return Accessor.Get<int?>(DistrictId); } }
        public string requesterType { set { RequesterType = Accessor.Set(value); } get { return Accessor.Get<int?>(RequesterType); } }
        
        public long? Code { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
        public string LocationDesc { get; set; }
        public string Description { get; set; }
        public string RequiredSupport { get; set; }
        public User User { get; set; }

        public ExpressionStarter<Models.EidFiterRequest> EntitiesFilteration()
        {
            var filter = PredicateBuilder.New<Models.EidFiterRequest>(true);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.EidFiterRequest, Dtos.EidFiterRequest>()

                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int UserId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Dtos.EidFiterRequest, Models.EidFiterRequest>()
                .IgnoreNullValues(true)

                .Config;
        }

        public TypeAdapterConfig AddConfig(int userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Dtos.EidFiterRequest, Models.EidFiterRequest>()
                .IgnoreNullValues(true)
                .Map(dest => dest.Code, src => Strings.RandomDigits(DateTime.Now, 1, 9999999999))
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Config;
        }

    }
    public class User
    {
        public string Phone { get; set; }
        public string Email { get; set; }
    }

}
