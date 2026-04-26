
using RM.ContactUs.Dtos;
using RM.Core.Services;
namespace RM.ContactUs.Services
{
    public interface IFollowUpContactUsService:IBaseService
    {
        Task<OperationOutput> GetContactListForManager(Dtos.ContactUs RequestedData);

        OperationOutput AddAction(Dtos.Action action);


        Task<OperationOutput> GetContactListForFollowUpUser(Dtos.ContactUs RequestedData);
        OperationOutput GetFollowUpOfficerLookup();
        Task<OperationOutput> GetComplaintListForProccessorUser(Dtos.ContactUs RequestedData);

        OperationOutput GetProccessorUserLookup();

        OperationOutput GetContactActions(Dtos.ContactUs RequestedData);


        Task<OperationOutput> AddEvaluation(Dtos.Feedback RequestedData);

        Task<OperationOutput> GetContactListForQualityAssurance(Dtos.ContactUs RequestedData);

        OperationOutput GetContactFeedbacks(Dtos.ContactUs RequestedData);
    }
}
