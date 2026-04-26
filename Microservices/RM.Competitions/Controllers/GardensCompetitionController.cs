using Mapster;
using Microsoft.AspNetCore.Mvc;
using RM.Competitions.Dtos;
using RM.Competitions.Records;
using RM.Competitions.Services;

namespace RM.Competitions.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GardensCompetitionController 
    {
        ICompetitionService _competitionService;
        public GardensCompetitionController(ICompetitionService competitionService)
        {
            _competitionService = competitionService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetLookups()
        {
            return await _competitionService.GetLookups();
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput Registration(RegistrationRecord RequestedData)
        {
            return _competitionService.Registration(RequestedData.Adapt<Dtos.Competitors>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> CompetitorsCandidates(CompetitorsCandidatesRecord RequestedData)
        {
            return await _competitionService.CompetitorsCandidates(RequestedData.Adapt<Dtos.Competitors>());
        } 

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetCompetitorsList(GetCompetitorsListRecord RequestedData)
        {
            return _competitionService.GetCompetitorsList(RequestedData.Adapt<Dtos.Competitors>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetCompetitorsDetails(GetCompetitorsDetailsRecord RequestedData)
        {
           return await _competitionService.GetCompetitorsDetails(RequestedData.Adapt<Dtos.Competitors>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput LoginOTP(LoginOTPRecord RequestedData)
        {
           return _competitionService.LoginOTP(RequestedData.Adapt<Dtos.Users>());
        } 

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput OTPVerification(Dtos.Auth RequestedData)
        {
           return _competitionService.OTPVerification(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetCompetitorsStatistics()
        {
           return _competitionService.GetCompetitorsStatistics();
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput CompleteAwardRequirements(Dtos.Attachments RequestedData)
        {
           return _competitionService.CompleteAwardRequirements(RequestedData);
        }
    }
}
