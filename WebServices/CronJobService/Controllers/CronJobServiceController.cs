using CronJobService.Services;
using Microsoft.AspNetCore.Mvc;
using CronJobService.Dtos;
using CronJobService.Records;
namespace CronJobService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CronJobServiceController : ControllerBase
    {
        private readonly ICronSettingsService _cronSettingsService;

        public CronJobServiceController(ICronSettingsService cronSettingsService)
        {
            _cronSettingsService = cronSettingsService;
        }

        [HttpPost]
        public async Task<OperationOutput> DoWork(DoWorkRecord RequestedData)
        {
           return await _cronSettingsService.DoWork(RequestedData.CronTypeId);
        }
           
    }

}
