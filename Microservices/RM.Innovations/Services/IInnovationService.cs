
using RM.Core.Services;
using RM.Innovations.Dtos;

namespace RM.Innovations.Services
{
    public interface IInnovationService: IBaseService
    {
        Task<OperationOutput> Save(Dtos.Ideas RequestedData);
        Task<OperationOutput> AddIdea940(Dtos.Ideas RequestedData);
        Task<OperationOutput> AddPublicIdea(Dtos.Ideas RequestedData);
        Task<OperationOutput> AddIdea(Dtos.Ideas RequestedData);
        Task<OperationOutput> GetUserIdeas();
        Task<OperationOutput> GetIdeasList(Dtos.Ideas RequestedData);
        Task<OperationOutput> GetPublicIdeasList(Dtos.Ideas RequestedData);
        Task<OperationOutput> GetIdeasLookups();
        Task<OperationOutput> GetIdeasDetails(Dtos.Ideas RequestedData);
        Task<OperationOutput> GetIdeasDetails(int? Id);
        Task<OperationOutput> SaveIdeaDetail(Dtos.Ideas RequestedData);
        Task<OperationOutput> IdeaAction(Dtos.Ideas RequestedData);
        Task<OperationOutput> SendingEmailToBeneficiary(string UserName, string UserEmail, long IdeaCode);
        byte[] Export(Dtos.Ideas RequestedData);
        Task<OperationOutput> IdeaAgreement(Dtos.Ideas RequestedData);
        Task<OperationOutput> IdeaChangeAgreement(Dtos.Ideas RequestedData);
        Task<OperationOutput> IdeasStatisticCounter(Dtos.Ideas RequestedData);
        Task<OperationOutput> TransferSuggestionToInnovation(Dtos.ContactUs RequestedData);
    }
}
