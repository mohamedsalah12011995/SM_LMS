
using Microsoft.AspNetCore.Mvc;
using RM.Volunteers.Dtos;
using RM.Volunteers.Services;
using Microsoft.AspNetCore.Authorization;
using RM.Core.Helpers;
using RM.Volunteers.Records;
using Mapster;

namespace RM.Volunteers.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VolunteersController
    {

        private readonly IVolunteersService _volunteerService;
        public VolunteersController( IVolunteersService volunteerService)
        {
            _volunteerService = volunteerService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveVolunteers(SaveVolunteersRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<Dtos.Volunteers>();
            RequestedData.EntityId = (int)Enums.Entities.Volunteers;
            return await _volunteerService.SaveVolunteers(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetLookups()
        {
            return await _volunteerService.GetVolunteersLookups();
        }
        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetVolunteersList(GetVolunteersListRecord RequestedData)
        {
            return await _volunteerService.GetVolunteersList(RequestedData.Adapt<Dtos.Volunteers>());
        }
        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetVolunteersDetails(GetVolunteersDetailsRecord RequestedData)
        {
            return await _volunteerService.GetVolunteersDetails(RequestedData.Adapt<Dtos.Volunteers>());
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
        {
            return _volunteerService.GetPathOfResource(fileName);
        }

    }
}
