using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using RM.Core.Helpers;
using RM.Investments.Dtos;
using RM.Investments.Records;
using RM.Investments.Services;

namespace RM.Investments.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InvestmentsController 
    {
        private readonly  IInvestmentsService _investmentsService;

        public InvestmentsController(IInvestmentsService investmentsService)
        {
            _investmentsService = investmentsService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetInvestmentsTypes()
        {
            return await _investmentsService.GetInvestmentsTypes();
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetInvestmentDetails(GetInvestmentDetailsRecord RequestedData)
        {
            return  await _investmentsService.GetInvestmentDetails(RequestedData.Adapt<Dtos.Investments>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveInvestment(SaveInvestmentRecord RequestedData)
        {
            return  await _investmentsService.SaveInvestment(RequestedData.Adapt<Dtos.Investments>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetInvestmentsOpportunitiesList(GetInvestmentsOpportunitiesListRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<Dtos.Investments>();
            RequestedData.EntityId = (int)Enums.Entities.InvestmentsOpportunities;
            return  await _investmentsService.GetInvestmentsList(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetInvestmentsCompetitionsList(GetInvestmentsCompetitionsListRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<Dtos.Investments>();
            RequestedData.EntityId = (int)Enums.Entities.InvestmentsCompetitions;
            return  await _investmentsService.GetInvestmentsList(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> Activation(ActivationRecord RequestedData)
        {
            return await _investmentsService.ModelAction(RequestedData.Adapt<Dtos.Investments>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> Delete(DeleteRecord RequestedData)
        {
            return await _investmentsService.ModelAction(RequestedData.Adapt<Dtos.Investments>());
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
        {
            return _investmentsService.GetPathOfResource(fileName);
        }
    }
}
