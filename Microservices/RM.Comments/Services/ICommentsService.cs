using RM.Comments.Dtos;

namespace RM.Comments.Services
{
    public interface ICommentsService
    {
        OperationOutput InsertComment(Dtos.Comments RequestedData);
        OperationOutput ApproveRejectComment(List<Dtos.Comments> comments);
        OperationOutput AddReply(Dtos.Comments RequestedData);
        Task<OperationOutput> GetCommentsListAsync(Dtos.Comments RequestedData);
        Task<OperationOutput> GetCommentsListByEntityAsync(Dtos.Comments RequestedData);
        OperationOutput GetUserEntities();
        OperationOutput GetCommentDetails(Dtos.Comments RequestedData);
    }
}
