using Microsoft.AspNetCore.Mvc;
using RM.Events.Dtos;
using RM.Events.Services;

namespace RM.Events.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EventsController
    {
        private readonly IEidFiterRequestService _eidFiterRequestService;
        public EventsController(IEidFiterRequestService eidFiterRequestService)
        {
            _eidFiterRequestService = eidFiterRequestService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> NewRequest(Dtos.EidFiterRequest RequestedData)
        {
            return await _eidFiterRequestService.Save(RequestedData);
        }
        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetLookups()
        {
            return await _eidFiterRequestService.GetLookups();
        }
    }
}
