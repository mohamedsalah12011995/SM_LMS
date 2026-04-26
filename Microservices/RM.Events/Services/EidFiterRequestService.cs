
using RM.Core.Services;
using RM.Events.UnitOfWorks;
using RM.Events.Dtos;
using RM.Core.Helpers;
using Mapster;
using static RM.Events.Dtos.OperationOutput;
using System.Data.Entity;
using RM.Models;

namespace RM.Events.Services
{
    public class EidFiterRequestService:BaseService, IEidFiterRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EidFiterRequestService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> Save(Dtos.EidFiterRequest RequestedData)
        {            
            Models.EidFiterRequest DbItem = new Models.EidFiterRequest();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

            RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id.Value));
            _unitOfWork.EidFiterRequests.Add(DbItem);

            if (RequestedData.User != null)
            {
                var User = _unitOfWork.User.GetById(RequestOwner.Id.Value);
                if (User != null && Strings.CheckSaudiMobileNumber(RequestedData.User.Phone) && Strings.CheckEmailValidity(RequestedData.User.Email))
                {
                    User.Email = RequestedData.User.Email;
                    User.Phone = RequestedData.User.Phone;
                    _unitOfWork.User.Update(User);
                }
            }

            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
        public async Task<OperationOutput> GetEdiFitrRequest(Dtos.EidFiterRequest RequestedData)
        {
            var Item = await _unitOfWork.EidFiterRequests.GetAll()
                             .AsNoTracking()
                             .FirstOrDefaultAsync(x => x.Id == RequestedData.Id.Value);

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = Item.Adapt<Dtos.EidFiterRequest>(Dtos.EidFiterRequest.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.EventEntity, ItemDto));
        }

        public async Task<OperationOutput> GetLookups()
        {
            var RequesterType = _unitOfWork.MajorLookup.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.RequesterType).ToList().Adapt<List<Dtos.Lookup>>();
            var Districts = _unitOfWork.MajorLookup.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.District && x.ParentId==(int)Enums.SpecificCities.Riyadh).ToList().Adapt<List<Dtos.Lookup>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.RequesterType, RequesterType),
                    new OutputDictionary(OperationOutputKeys.Districts, Districts));

        }

    }
}
