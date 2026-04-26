using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Comments.Dtos;
using RM.Comments.UnitOfWorks;
using RM.Core.Helpers;
using RM.Core.Services;
using static RM.Comments.Dtos.OperationOutput;
using RM.Models;
using RM.Core.Consts;

namespace RM.Comments.Services
{
    public class CommentsService: BaseService, ICommentsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }
        public OperationOutput InsertComment(Dtos.Comments RequestedData)
        {
            Comment DbItem = new Comment();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id));
            _unitOfWork.Comments.Add(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.CommentInsertedSuccess,
                (int)Enums.ServiceMessages.TransactionSuccess);
        }
        public OperationOutput ApproveRejectComment(List<Dtos.Comments> comments)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (comments.Any())
            {
                var commentList = _unitOfWork.Comments.GetAll()
                    .Where(t => comments.Select(x => x.Id).Contains(t.Id)).AsNoTracking().ToList();

                if (commentList.Any())
                {
                    foreach (var model in commentList)
                    {
                        var comment = comments.Find(c => c.Id == model.Id);
                        model.IsApproved = comment.IsApproved.HasValue ? comment.IsApproved : model.IsApproved;
                        model.ApprovedBy = RequestOwner.Id;
                        _unitOfWork.Comments.Update(model);
                    }
                    _unitOfWork.Complete();
                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

                }
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

        }
        public OperationOutput AddReply(Dtos.Comments RequestedData)
        {

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var DbItem = _unitOfWork.Comments.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            DbItem.ReplyText = RequestedData.ReplyText;
            DbItem.ReplyDate = TransactionDate;
            DbItem.RepliedBy = RequestOwner.Id;

            _unitOfWork.Comments.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }
        public async Task<OperationOutput> GetCommentsListAsync(Dtos.Comments RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var filteration = RequestedData.Filteration();

            var comments = await _unitOfWork.Comments.FindAllByPaginationAsync(filteration, RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Descending,
                 x => x.RepliedByNavigation);

            var commentsDto = comments.Data.Adapt<List<Dtos.Comments>>(Dtos.Comments.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,null,
                   new OutputDictionary(OperationOutputKeys.CommentsEntity, commentsDto),
                   new OutputDictionary(OperationOutputKeys.Pagination, comments.Pagination));
        }


        public async Task<OperationOutput> GetCommentsListByEntityAsync(Dtos.Comments RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var filteration = RequestedData.Filteration();

            var comments = await _unitOfWork.Comments.FindAllByPaginationAsync(filteration, RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Descending,
                 x => x.RepliedByNavigation);

            var commentsDto = comments.Data.Adapt<List<Dtos.Comments>>(Dtos.Comments.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,null,
                   new OutputDictionary(OperationOutputKeys.CommentsEntity, commentsDto),
                   new OutputDictionary(OperationOutputKeys.Pagination, comments.Pagination));
        }

        public OperationOutput GetUserEntities()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var userEntities = _unitOfWork.UsersEntities.GetAll()
                .Include(n => n.Entity)
                .Where(x => x.UserId == RequestOwner.Id)
                .OrderBy(x => x.Id).AsNoTracking();

            var userEntitiesDto = userEntities.Adapt<List<Dtos.EntitiesItem>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,null,
            new OutputDictionary(OperationOutputKeys.EntitiesItem, userEntitiesDto));
        }

        public OperationOutput GetCommentDetails(Dtos.Comments RequestedData)
        {
            Dtos.Comments ItemDto = new Dtos.Comments();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Item = _unitOfWork.Comments.GetAll()
                        .Include(x => x.RepliedByNavigation)
                        .Where(x => x.Id == RequestedData.Id)
                        .AsNoTracking().FirstOrDefault();

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            Item.Adapt(ItemDto, Dtos.Comments.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,null,
                   new OutputDictionary(OperationOutputKeys.CommentsEntity, ItemDto));
        }
    }
}
