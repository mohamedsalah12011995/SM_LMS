
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.Awards.Dtos;
using RM.Awards.Records;
using RM.Awards.Services;
namespace RM.Awards.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AwardsController
    {
        private readonly IAwardsService _awardService;
        public AwardsController(IAwardsService awardService)
        {
            _awardService = awardService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveAwards(SaveAwardsRecord RequestedData)
        {
            return await _awardService.SaveAwards(RequestedData.Adapt<Dtos.Awards>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> GetAwardsList(GetAwardsListRecord RequestedData)
        {
            return await _awardService.GetAwardsList(RequestedData.Adapt<Dtos.Awards>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetAwardsDetails(GetAwardsDetailsRecord RequestedData)
        {
            return await _awardService.GetAwardsDetails(RequestedData.Adapt<Dtos.Awards>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput Activation(ActivationRecord RequestedData)
        {
            return _awardService.ModelActions(RequestedData.Adapt<Dtos.Awards>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput Delete(DeleteRecord RequestedData)
        {
            return _awardService.ModelActions(RequestedData.Adapt<Dtos.Awards>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput SortOrder(List<SortOrderRecord> RequestedData)
        {
            return _awardService.SortOrder(RequestedData.Adapt<List<Dtos.Awards>>());
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
        {
            return _awardService.GetPathOfResource(fileName);
        }

    }
}
