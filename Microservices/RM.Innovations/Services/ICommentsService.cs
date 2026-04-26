
using RM.Innovations.Dtos;

namespace RM.Innovations.Services
{
    public interface ICommentsService
    {
        Task<OperationOutput> InsertComment(Dtos.IdeaComments RequestedData);
        Task<OperationOutput> ApproveRejectComment(Dtos.IdeaComments RequestedData);
        Task<OperationOutput> AddReply(Dtos.IdeaComments RequestedData);
        Task<OperationOutput> GetCommentsList(Dtos.IdeaComments RequestedData);
        Task<OperationOutput> GetCommentDetails(Dtos.IdeaComments RequestedData);
        Task<OperationOutput> DeleteComment(Dtos.IdeaComments RequestedData);
    }
}
