
using Microsoft.AspNetCore.Mvc;
using RM.Partners.Dtos;
using RM.Partners.Services;
using Microsoft.AspNetCore.Authorization;
using RM.Core.Helpers;
using RM.Partners.Records;
using Mapster;


namespace RM.Partners.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PartnersController 
    {
        private readonly IPartnersService _partnersService;
        public PartnersController(IPartnersService partnersService, IConfiguration configuration)
        {
            _partnersService = partnersService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SavePartners(SavePartnersRecord RequestedData)
        {
           return await _partnersService.SavePartners(RequestedData.Adapt<Dtos.Partner>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetPartners(GetPartnersRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<Dtos.Partner>();
            RequestedData.EntityId = (int)Enums.Entities.Partners;
            return await _partnersService.GetPartners(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetLocalPartners(GetLocalPartnersRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<Dtos.Partner>();
            RequestedData.EntityId = (int)Enums.Entities.Partners_Local;
            return await _partnersService.GetPartners(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetPartnerDetails(GetPartnerDetailsRecord RequestedData)
        {
            return await _partnersService.GetPartnerDetails(RequestedData.Adapt<Dtos.Partner>());
        }

        [HttpPost]
        [Produces("application/json")]
        public Task<OperationOutput> Activation(ActivationRecord RequestedData)
        {
           return _partnersService.ModelAction(RequestedData.Adapt<Dtos.Partner>());
        }

        [HttpPost]
        [Produces("application/json")]
        public Task<OperationOutput> Delete(DeleteRecord RequestedData)
        {
           return _partnersService.ModelAction(RequestedData.Adapt<Dtos.Partner>());
        }


        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
        {
            return _partnersService.GetPathOfResource(fileName);
        }
    }
}
