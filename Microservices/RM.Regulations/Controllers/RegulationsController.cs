using Mapster;
using Microsoft.AspNetCore.Mvc;
using RM.Regulations.Dtos;
using RM.Regulations.Records;
using RM.Regulations.Services;

namespace RM.Regulations.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegulationsController : ControllerBase
    {
        private readonly IRegulationService _regulationService;
        public RegulationsController(IRegulationService regulationService)
        {
            _regulationService = regulationService;
        }


        [HttpPost]

        public async Task<OperationOutput> GetRegulationsLookups(GetRegulationsLookupsRecord RequestedData)
             => await _regulationService.GetRegulationsLookups(RequestedData.Adapt<Dtos.Regulations>());


        [HttpPost]

        public async Task<OperationOutput> GetRegulationsList(GetRegulationsListRecord RequestedData)
             => await _regulationService.GetRegulationsList(RequestedData.Adapt<Dtos.Regulations>());

        [HttpPost]

        public async Task<OperationOutput> SaveRegulationCategory(SaveRegulationCategoryRecord RequestedData)
             => await _regulationService.SaveRegulationCategory(RequestedData.Adapt<Dtos.Regulations>());

        [HttpPost]

        public async Task<OperationOutput> SaveRegulations(SaveRegulationsRecord RequestedData)
           => await _regulationService.SaveRegulations(RequestedData.Adapt<Dtos.Regulations>());

        [HttpPost]

        public async Task<OperationOutput> GetRegulationDetails(GetRegulationDetailsRecord RequestedData)
            => await _regulationService.GetRegulationDetails(RequestedData.Adapt<Dtos.Regulations>());

        [HttpPost]
        public async Task<OperationOutput> Activation(ActivationRecord RequestedData)
            => await _regulationService.ModelAction(RequestedData.Adapt<Dtos.Regulations>());

        [HttpPost]
        public async Task<OperationOutput> Delete(DeleteRecord RequestedData)
            => await _regulationService.ModelAction(RequestedData.Adapt<Dtos.Regulations>());
    }
}
