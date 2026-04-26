using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Regulations.Dtos;
using RM.Regulations.Integrations;
using RM.Regulations.UnitOfWorks;
using static RM.Regulations.Dtos.OperationOutput;

namespace RM.Regulations.Services
{
    public class RegulationService : BaseService, IRegulationService
    {

        private readonly IUnitOfWork _unitOfWork;

        public RegulationService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetRegulationsLookups(Dtos.Regulations RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var regulations = await _unitOfWork.TermsAndRegulation.GetAll()
                 .Where(x => x.IsDeleted != true && !x.ParentId.HasValue)
                 .Where(x => RequestedData.ReferenceId.HasValue ? x.ReferenceId == RequestedData.ReferenceId : true)
                 .AsNoTracking().ToListAsync();

            var regulationsDto = regulations.Adapt<List<Dtos.Regulations>>(Dtos.Regulations.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKey.RegulationsEntity, regulationsDto));
        }

        public async Task<OperationOutput> GetRegulationsList(Dtos.Regulations RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;

            var regulations = await _unitOfWork.TermsAndRegulation.FindAllByPagination(RequestedData.Filteration(),
                             RequestedData.Pagination, DefaultPaginationCount, x => x.CreatedDate,
                             OrderBy.Descending, x => x.CreatedByNavigation, c => c.UpdatedByNavigation, p => p.Parent);

            var regulationsDto = regulations.Data.Adapt<List<Dtos.Regulations>>(Dtos.Regulations.SelectConfig());

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, RequestedData.ReferenceId, (int)Enums.Entities.Regulations, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKey.RegulationsEntity, regulationsDto),
                new OutputDictionary(OperationOutputKey.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.Regulations)),
                new OutputDictionary(OperationOutputKey.Pagination, regulations.Pagination));

        }
        public async Task<OperationOutput> GetRegulationDetails(Dtos.Regulations RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var regulation = await _unitOfWork.TermsAndRegulation.GetAll()
                            .AsNoTracking().FirstOrDefaultAsync(x => x.Id == RequestedData.Id);

            var regulationDto = regulation.Adapt<Dtos.Regulations>
                                (Dtos.Regulations.SelectConfig());


            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, regulationDto.ReferenceId, (int)Enums.Entities.Regulations, regulationDto.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKey.RegulationsEntity, regulationDto),
               new OutputDictionary(OperationOutputKey.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.Regulations)));

        }

        public async Task<OperationOutput> SaveRegulationCategory(Dtos.Regulations RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.TermsAndRegulation regulation = new();

            if (RequestedData.Id.HasValue)
            {
                regulation = _unitOfWork.TermsAndRegulation.GetById(RequestedData.Id.Value);

                if (regulation is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(regulation, RequestedData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.TermsAndRegulation.Update(regulation);
            }
            else
            {
                RequestedData.Adapt(regulation, RequestedData.AddConfig(RequestOwner.Id, isCategory: true));
                _unitOfWork.TermsAndRegulation.Add(regulation);
            }

            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKey.Id, Accessor.Get(regulation.Id)));
        }

        public async Task<OperationOutput> SaveRegulations(Dtos.Regulations RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.TermsAndRegulation regulation = new();

            if (RequestedData.Id.HasValue)
            {
                regulation = _unitOfWork.TermsAndRegulation.GetById(RequestedData.Id.Value);

                if (regulation is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(regulation, RequestedData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.TermsAndRegulation.Update(regulation);
            }
            else
            {
                RequestedData.Adapt(regulation, RequestedData.AddConfig(RequestOwner.Id, isCategory: false));
                _unitOfWork.TermsAndRegulation.Add(regulation);
            }

            await _unitOfWork.CompleteAsync();

            return await GetRegulationDetails(new Dtos.Regulations { Id = regulation.Id });

        }
        public async Task<OperationOutput> ModelAction(Dtos.Regulations RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (RequestedData.IsDeleted.HasValue)
            {
                await _unitOfWork.TermsAndRegulation.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                       sett => sett.SetProperty(x => x.IsDeleted, RequestedData.IsDeleted.Value)
                       .SetProperty(d => d.DeletedBy, RequestOwner.Id)
                       .SetProperty(y => y.DeletedDate, TransactionDate)
                       .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                       .SetProperty(y => y.UpdatedDate, TransactionDate));
            }
            if (RequestedData.IsActive.HasValue)
            {
                await _unitOfWork.TermsAndRegulation.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                        sett => sett.SetProperty(x => x.IsActive, RequestedData.IsActive.Value)
                        .SetProperty(d => d.ActivatedBy, RequestOwner.Id)
                        .SetProperty(y => y.ActivatedDate, TransactionDate)
                        .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                        .SetProperty(y => y.UpdatedDate, TransactionDate));
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
    }
}
