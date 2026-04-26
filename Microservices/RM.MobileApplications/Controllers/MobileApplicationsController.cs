
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.MobileApplications.Dtos;
using RM.MobileApplications.Records;
using RM.MobileApplications.Services;


namespace RM.MobileApplications.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MobileApplicationsController 
    {
        private readonly  IMobileApplicationsService _mobileApplicationsService;
        public MobileApplicationsController(IMobileApplicationsService mobileApplicationsService)
        {
            _mobileApplicationsService = mobileApplicationsService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetMobileApplicationList(GetMobileApplicationListRecord RequestedData)
        {
            return await _mobileApplicationsService.GetMobileApplicationList(RequestedData.Adapt<Dtos.MobileApplications>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetMobileApplicationDetails(GetMobileApplicationDetailsRecord RequestedData)
        {
            return await _mobileApplicationsService.GetMobileApplicationDetails(RequestedData.Adapt<Dtos.MobileApplications>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveApplicationInformation(SaveApplicationInformationRecord RequestedData)
        {
            return await _mobileApplicationsService.SaveApplicationInformation(RequestedData.Adapt<Dtos.MobileApplications>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> Activation(ActivationRecord RequestedData)
        {
            return await _mobileApplicationsService.ModelAction(RequestedData.Adapt<Dtos.MobileApplications>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> Delete(DeleteRecord RequestedData)
        {
            return await _mobileApplicationsService.ModelAction(RequestedData.Adapt<Dtos.MobileApplications>());
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
        {
            return _mobileApplicationsService.GetPathOfResource(fileName);
        }
    }
}
