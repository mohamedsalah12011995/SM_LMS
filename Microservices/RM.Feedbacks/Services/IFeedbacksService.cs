
using RM.Feedbacks.Dtos;

namespace RM.Feedbacks.Services
{
    public interface IFeedbacksService
    {
        Task<OperationOutput> GetFeedbacksLookups(Dtos.Feedbacks RequestedData);
        Task<OperationOutput> GetFeedbacksList(Dtos.Feedbacks RequestedData);
        Task<OperationOutput> GetFeedbacksDetails(Dtos.Feedbacks RequestedData);
        Task<OperationOutput> GetFeedbacksDetails(int Id);
        Task<OperationOutput> SaveFeedbacks(Dtos.Feedbacks RequestedData);
        Task<OperationOutput> FeedbacksModelActions(Dtos.Feedbacks RequestedData);
    }
}
