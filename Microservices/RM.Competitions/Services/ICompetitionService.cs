
using RM.Competitions.Dtos;
using RM.Core.Helpers;
using RM.Models.Competitions;

namespace RM.Competitions.Services
{
    public interface ICompetitionService
    {
        OperationOutput Registration(Dtos.Competitors RequestedData);
        Task<OperationOutput> CompetitorsCandidates(Dtos.Competitors RequestedData);
        OperationOutput GetCompetitorsList(Dtos.Competitors RequestedData);
        Task<OperationOutput> GetCompetitorsDetails(Dtos.Competitors RequestedData);
        OperationOutput LoginOTP(Dtos.Users RequestedData);
        OperationOutput OTPVerification(Dtos.Auth RequestedData);
        OperationOutput GetCompetitorsStatistics();
        OperationOutput CompleteAwardRequirements(Dtos.Attachments RequestedData);
        Task<OperationOutput> GetLookups();
        OperationOutput SaveAttachment(ref Competitor CompetitorItem, List<Dtos.Attachments.AttachmentBase> AttachList, Enums.AttachmentsType Type);

    }
}
