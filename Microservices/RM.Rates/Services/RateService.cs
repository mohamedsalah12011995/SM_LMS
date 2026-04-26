using Mapster;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Rates.Dtos;
using RM.Rates.UnitOfWorks;

namespace RM.Rates.Services
{
    public class RateService : BaseService, IRateService
    {


        private readonly IUnitOfWork _unitOfWork;
        public RateService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationOutput> InsertRates(Dtos.Rates RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);
            Models.Rate Rate = new();

            RequestedData.Adapt(Rate, RequestedData.AddConfig(RequestOwner.Id));
            _unitOfWork.Rates.Add(Rate);
            await _unitOfWork.CompleteAsync();

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }
    }
}
