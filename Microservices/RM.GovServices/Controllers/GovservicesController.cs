
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using RM.GovServices.Services;
using RM.GovServices.Dtos;
using RM.GovServices.Records;
using Mapster;

namespace RM.GovServices.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GovservicesController 
    {
        private readonly IGovServicesService _govservicesService;
        public GovservicesController(ILogger<GovservicesController> logger, IGovServicesService govservicesService)
        {
            _govservicesService = govservicesService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetGovserviceList(GetGovserviceListRecord RequestedData)
        {
            return await _govservicesService.GetGovserviceList(RequestedData.Adapt<Dtos.Govservice>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetEserviceList(GetEserviceListRecord RequestedData)
        {
            return await _govservicesService.GetEserviceList(RequestedData.Adapt<Dtos.Eservices>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetGovserviceDetails(GetGovserviceDetailsRecord RequestedData)
        {
            return await _govservicesService.GetGovserviceDetails(RequestedData.Adapt<Dtos.Govservice>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetEserviceDetails(GetEserviceDetailsRecord RequestedData)
        {
            return await _govservicesService.GetEserviceDetails(RequestedData.Adapt<Dtos.Eservices>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveGovService(SaveGovServiceRecord RequestedData)
        {
            return await _govservicesService.SaveGovService(RequestedData.Adapt<Dtos.Govservice>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveEservice(SaveEserviceRecord RequestedData)
        {
            return await _govservicesService.SaveEservice(RequestedData.Adapt<Dtos.Eservices>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetGovServicesCategories(GetGovServicesCategoriesRecord RequestedData)
        {
            return await _govservicesService.GetGovServicesCategories(RequestedData.Adapt<Dtos.Govservice>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetEservicesCategories(GetEservicesCategoriesRecord RequestedData)
        {
            return await _govservicesService.GetEservicesCategories(RequestedData.Adapt<Dtos.Govservice>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GovServiceActivation(GovServiceActivationRecord RequestedData)
        {
            return await _govservicesService.GovServicesModelActions(RequestedData.Adapt<Dtos.Govservice>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GovServiceDelete(GovServiceDeleteRecord RequestedData)
        {
            return await _govservicesService.GovServicesModelActions(RequestedData.Adapt<Dtos.Govservice>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> EServiceActivation(EServiceActivationRecord RequestedData)
        {
            return await _govservicesService.EservicesModelActions(RequestedData.Adapt<Dtos.Eservices>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> EServiceDelete(EServiceDeleteRecord RequestedData)
        {
            return await _govservicesService.EservicesModelActions(RequestedData.Adapt<Dtos.Eservices>());
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
        {
            return _govservicesService.GetPathOfResource(fileName);
        }

    }
}
