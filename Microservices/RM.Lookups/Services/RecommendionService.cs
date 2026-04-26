using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RM.Lookups.Dtos;
using RM.Lookups.UnitOfWorks;
using RM.Core.Helpers;
using RM.Core.Services;
using Mapster;

namespace RM.Lookups.Services
{
    public class RecommendionService : BaseService, IRecommendionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecommendionService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetLookups(Recommendations RequestedData)
        {

            var Entities = _unitOfWork.Entity.GetAll().AsNoTracking().ToList();
            var EntitiesDto = Entities.Adapt<List<Dtos.Recommendations>>();
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutput.OperationOutputKeys.Entities, EntitiesDto));
        }
        public async Task<OperationOutput> GetAllAsync(Recommendations dto)
        {
            var filteration = dto.Filteration();

            var RecommendationsList = await _unitOfWork.Recommendations.GetAll().Where(filteration)
                                    .Include(c => c.CreatedByNavigation).Include(u => u.UpdatedByNavigation)
                                    .AsNoTracking().TakePagginationAsync(dto.Pagination, DefaultPaginationCount);

            var RecommendationsDtos = RecommendationsList.Data.Adapt<List<Recommendations>>(Recommendations.SelectConfig());

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutput.OperationOutputKeys.Recommendions, RecommendationsDtos),
               new OutputDictionary(OperationOutput.OperationOutputKeys.Pagination, RecommendationsList.Pagination));
        }

        public async Task<OperationOutput> GetAsync(Recommendations dto)
        {
            return await GetAsync(dto.Id);
        }


        private async Task<OperationOutput> GetAsync(int? Id)
        {
            var Item = await _unitOfWork.Recommendations.GetAll().Where(x => x.Id == Id)
                .AsNoTracking().FirstOrDefaultAsync();

            if (Item == null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = Item.Adapt(new Recommendations(), Recommendations.SelectConfig());

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutput.OperationOutputKeys.Recommendions, ItemDto));
        }

        public async Task<OperationOutput> AddAsync(Recommendations dto)
        {
            var Item = dto.Adapt(new RM.Models.Recommendations(), dto.AddConfig(RequestOwner.Id));
            _unitOfWork.Recommendations.Add(Item);
            await _unitOfWork.CompleteAsync();
            return await GetAsync(Item.Id);
        }


        public async Task<OperationOutput> UpdateAsync(Recommendations dto)
        {
            var Item = await _unitOfWork.Recommendations.GetAll().Where(x => x.Id == dto.Id).FirstOrDefaultAsync();
            if (Item == null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            dto.Adapt(Item, dto.UpdateConfig(RequestOwner.Id));

            _unitOfWork.Recommendations.Update(Item);
            await _unitOfWork.CompleteAsync();

            return await GetAsync(Item.Id);
        }

        public async Task<OperationOutput> DeleteAsync(Recommendations dto)
        {

            var Item = await _unitOfWork.Recommendations.GetByIdAsync(dto.Id.Value);
            if (Item == null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            _unitOfWork.Recommendations.Delete(Item);

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }


    }
}
