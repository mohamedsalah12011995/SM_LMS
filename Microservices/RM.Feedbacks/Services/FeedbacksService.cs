

using Microsoft.EntityFrameworkCore;
using RM.Core.Services;
using RM.Core.Helpers;
using RM.Core.Consts;
using Mapster;
using RM.Feedbacks.UnitOfWorks;
using RM.Feedbacks.Dtos;
using static RM.Feedbacks.Dtos.OperationOutput;

namespace RM.Feedbacks.Services
{
    public class FeedbacksService : BaseService, IFeedbacksService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FeedbacksService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetFeedbacksLookups(Dtos.Feedbacks RequestedData)
        {

            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Recommendions = await _unitOfWork.Recommendations.GetAll().Where(x => x.EntityId == (int)Enums.Entities.Feedbacks).AsNoTracking().ToListAsync();
            var surveyRecommendionsDto = Recommendions.Adapt<List<Recommendations>>(Recommendations.SelectConfig());

            var Feedbackss =  _unitOfWork.Feedbacks.GetAll().Where(RequestedData.Filteration()).AsNoTracking().ToList();

            var FeedbackssDto = Feedbackss.Adapt<List<Dtos.Feedbacks>>(Dtos.Feedbacks.SelectConfig());
            var GregorianYears = Enumerable.Range(2023, DateTime.Now.Year - 2022).ToList();
            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                                   new OutputDictionary(OperationOutputKey.FeedbacksEntity, FeedbackssDto),
                                   new OutputDictionary("YearsList", GregorianYears),
                                   new OutputDictionary("Recommendations", surveyRecommendionsDto));
        }

        public async Task<OperationOutput> GetFeedbacksList(Dtos.Feedbacks RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Feedbackss = await _unitOfWork.Feedbacks.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, 
                OrderBy.Descending, x => x.CreatedByNavigation, x => x.UpdatedByNavigation);

            var FeedbackssDto = Feedbackss.Data.Adapt<List<Dtos.Feedbacks>>(Dtos.Feedbacks.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.FeedbacksEntity, FeedbackssDto),
                   new OutputDictionary(OperationOutputKey.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.Feedbacks)),
                   new OutputDictionary(OperationOutputKey.Pagination, Feedbackss.Pagination));
        }
        public async Task<OperationOutput> GetFeedbacksDetails(Dtos.Feedbacks RequestedData)
        {
            return await GetFeedbacksDetails(RequestedData.Id.Value);
        }
        public async Task<OperationOutput> GetFeedbacksDetails(int Id)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

           var DbItem = _unitOfWork.Feedbacks.GetAll().Include(x => x.FeedbacksDataSources)
                .Where(x => x.Id == Id && x.IsDeleted != true)
                .AsNoTracking().FirstOrDefault();

            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

           var ItemDto = DbItem.Adapt<Dtos.Feedbacks>(Dtos.Feedbacks.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.FeedbacksEntity, ItemDto),
                   new OutputDictionary(OperationOutputKey.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.Feedbacks)));
        }

        public async Task<OperationOutput> SaveFeedbacks(Dtos.Feedbacks RequestdData)
        {
            Models.Feedbacks DbItem = new Models.Feedbacks();
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            using (var dbContextTransaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if (RequestdData.Id.HasValue)
                    {
                        DbItem = await _unitOfWork.Feedbacks.GetAll().Include(x => x.FeedbacksDataSources).Where(x => x.Id == RequestdData.Id.Value).FirstOrDefaultAsync();
                        if (DbItem == null)
                            return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);
                        
                        await _unitOfWork.Feedbacks.UpdateAsync(DbItem, RequestdData.Adapt(DbItem, RequestdData.UpdateConfig(RequestOwner.Id)));

                        foreach(var ds in DbItem.FeedbacksDataSources.Where(x => !RequestdData.FeedbacksDataSources.Where(z => z.Id.HasValue).Select(z => z.Id).ToList().Contains(x.Id)).ToList())
                        {
                            ds.IsDeleted = true;
                            _unitOfWork.FeedbacksDataSources.Update(ds);
                        }
                    }
                    else
                    {
                        RequestdData.Adapt(DbItem, RequestdData.AddConfig(RequestOwner.Id));
                        _unitOfWork.Feedbacks.Add(DbItem);
                    }

                    await _unitOfWork.CompleteAsync();

                    foreach (var ds in RequestdData.FeedbacksDataSources.Where(x=>x.IsDeleted != true))
                    {
                        if (!ds.Id.HasValue)
                        {
                            var NewItem = ds.Adapt(new RM.Models.FeedbacksDataSource(), ds.AddConfig(DbItem.Id));
                            await _unitOfWork.FeedbacksDataSources.AddAsync(NewItem);
                        }
                        else
                        {
                            ds.FeedbacksId = DbItem.Id;
                            await _unitOfWork.FeedbacksDataSources.UpdateAsync(DbItem.FeedbacksDataSources.Where(x => x.Id == ds.Id).FirstOrDefault(), ds);
                        }
                    }

                    await _unitOfWork.CompleteAsync();
                    dbContextTransaction.Commit();
                    return await GetFeedbacksDetails(DbItem.Id);
                }
                catch
                {
                    dbContextTransaction.Rollback();
                     return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
                }
            }
        }

        public async Task<OperationOutput> FeedbacksModelActions(Dtos.Feedbacks RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsDeleted.HasValue && !RequestedData.IsActive.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.Feedbacks.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            DbItem.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : DbItem.IsDeleted;
            DbItem.IsActive = RequestedData.IsActive.HasValue ? RequestedData.IsActive.Value : DbItem.IsActive;

            DbItem.UpdatedBy = RequestOwner.Id;
            DbItem.UpdatedDate = DateTime.Now;

            _unitOfWork.Feedbacks.Update(DbItem);
            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

    }
}
