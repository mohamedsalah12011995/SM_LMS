
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using RM.Innovations.Dtos;
using RM.Innovations.UnitOfWorks;
using RM.Core.Services;
using RM.Models;
using RM.Core.Integrations;
using RM.Core.Helpers;
using static RM.Innovations.Dtos.OperationOutput;
using Mapster;
using System.Linq;

namespace RM.Innovations.Services
{
    public class CommentsService:BaseService, ICommentsService
    {

        private readonly IUnitOfWork _unitOfWork;

        public CommentsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationOutput> InsertComment(Dtos.IdeaComments RequestedData)
        {
            IdeaComment DbItem = new Models.IdeaComment();

            if (UseCapcha)
                if (string.IsNullOrEmpty(RequestedData.Capcha) || !GoogleCapcha.CheckCapchaSession(CapchaSecret, RequestedData.Capcha))
                    return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            if (!RequestedData.IdeaId.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id));
            DbItem.IsApproved = false;

            _unitOfWork.IdeaComments.Add(DbItem);
            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.CommentInsertedSuccess);
        }
        public async Task<OperationOutput> ApproveRejectComment(Dtos.IdeaComments RequestedData)
        {
            var UserDbItem = _unitOfWork.Users.Find(x => x.Id == RequestOwner.Id);

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var UserJobRole = (Enums.JobRole)UserDbItem.JobRole;
            var DbItem = _unitOfWork.IdeaComments.GetById(RequestedData.Id.Value);

            if (DbItem == null || UserJobRole != Enums.JobRole.ContentManager)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

            DbItem.IsApproved = RequestedData.IsApproved.HasValue ? RequestedData.IsApproved : DbItem.IsApproved;
            DbItem.ApprovedBy = RequestOwner.Id;
            _unitOfWork.IdeaComments.Update(DbItem);
            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
        public async Task<OperationOutput> AddReply(Dtos.IdeaComments RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var DbItem = _unitOfWork.IdeaComments.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

            DbItem.ReplyText = RequestedData.ReplyText;
            DbItem.ReplyDate = TransactionDate;
            DbItem.RepliedBy = RequestOwner.Id;

            _unitOfWork.IdeaComments.Update(DbItem);
            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
        public async Task<OperationOutput> GetCommentsList(Dtos.IdeaComments RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);
         
            var result = _unitOfWork.IdeaComments.GetAll()
                .Include(c=>c.RepliedByNavigation)
                .Where(RequestedData.Filteration())
                .OrderByDescending(c=> c.Id).AsNoTracking().TakePaggination(RequestedData.Pagination, DefaultPaginationCount);

            var resultDto=result.Data.ToList().Adapt<List<IdeaComments>>(IdeaComments.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.CommentsEntity, resultDto),
                    new OutputDictionary(OperationOutputKeys.Pagination, result.Pagination));
        }

        public async Task<OperationOutput> GetCommentDetails(Dtos.IdeaComments RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Item = _unitOfWork.IdeaComments.GetAll()
                .Where(x => x.Id == RequestedData.Id)
                .Include(c => c.RepliedByNavigation).FirstOrDefault();

            if(Item == null) 
                return GetOperationOutput(header:Enums.ServiceMessages.NoDataReturned);

            var ItemDto = Item.Adapt<IdeaComments>(IdeaComments.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.CommentsEntity, ItemDto));
        }

        public async Task<OperationOutput> DeleteComment(Dtos.IdeaComments RequestedData)
        {
            var UserDbItem = _unitOfWork.Users.Find(x => x.Id == RequestOwner.Id);

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var UserJobRole = (Enums.JobRole)UserDbItem.JobRole;
            var DbItem = _unitOfWork.IdeaComments.GetById(RequestedData.Id.Value);

            if (DbItem == null || UserJobRole != Enums.JobRole.ContentManager)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

            _unitOfWork.IdeaComments.Delete(DbItem);
            await _unitOfWork.CompleteAsync();
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }
    }
}
