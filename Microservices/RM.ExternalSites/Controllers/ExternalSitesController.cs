using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using RM.ExternalSites.Services;
using RM.ExternalSites.Dtos;
using System.IO;
using System.Threading.Tasks;
using RM.ExternalSites.Records;
using Mapster;

namespace RM.ExternalSites.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExternalSitesController 
    {

        private readonly IExternalSitesService _externalSitesService;
        public ExternalSitesController(IExternalSitesService externalSitesService)
        {
            _externalSitesService = externalSitesService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetCategories(GetCategoriesRecord RequestedData)
        {
            return await _externalSitesService.GetCategories(RequestedData.Adapt<Dtos.ExternalSites>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetExternalSites(GetExternalSitesRecord RequestedData)
        {
            return await _externalSitesService.GetExternalSites(RequestedData.Adapt<Dtos.ExternalSites>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveExternalSite(SaveExternalSiteRecord RequestedData)
        {
            return await _externalSitesService.SaveExternalSite(RequestedData.Adapt<Dtos.ExternalSites>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> GetExternalSitesList(GetExternalSitesListRecord RequestedData)
        {
            return await _externalSitesService.GetExternalSitesList(RequestedData.Adapt<Dtos.ExternalSites>());
        }
        [HttpPost]
        [Produces("application/json")]
        public async  Task<OperationOutput> GetExternalSitesDetails(GetExternalSitesDetailsRecord RequestedData)
        {
            return await _externalSitesService.GetExternalSitesDetails(RequestedData.Adapt<Dtos.ExternalSites>());
        }
        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> Activation(ActivationRecord RequestedData)
        {
            return await _externalSitesService.ModelActions(RequestedData.Adapt<Dtos.ExternalSites>());
        }
        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> Delete(DeleteRecord RequestedData)
        {
            return await _externalSitesService.ModelActions(RequestedData.Adapt<Dtos.ExternalSites>());
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
        {
            return _externalSitesService.GetPathOfResource(fileName);

        }

    }



    
}
