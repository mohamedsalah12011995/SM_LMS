using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.Core.Helpers;
using RM.Officials.Dtos;
using RM.Officials.Records;
using RM.Officials.Services;

namespace RM.Officials.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OfficialsController : ControllerBase
    {

        private readonly IOfficialService _officialService;

        public OfficialsController(IOfficialService officialService)
           => _officialService = officialService;


        [HttpPost]

        public async Task<OperationOutput> GetOffcialDetails(GetOffcialDetailsRecord RequestedData)
            => await _officialService.GetOfficialDetails(RequestedData.Adapt<OfficialDto>());

        [HttpPost]

        public async Task<OperationOutput> SaveMayor(SaveMayorRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<OfficialDto>();
            RequestedData.EntityId = (int)Enums.Entities.Mayors;
            return await _officialService.SaveOfficial(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> SavePrince(SavePrinceRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<OfficialDto>();
            RequestedData.EntityId = (int)Enums.Entities.Princes;
            return await _officialService.SaveOfficial(RequestedData);
        }


        [HttpPost]

        public async Task<OperationOutput> SaveOfficials(SaveOfficialsRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<OfficialDto>();
            RequestedData.EntityId = (int)Enums.Entities.RmOfficials;
            return await _officialService.SaveOfficial(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetPrincesList(GetPrincesListRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<OfficialDto>();
            RequestedData.EntityId = (int)Enums.Entities.Princes;
            return await _officialService.GetOfficialsList(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetOfficialsList(GetOfficialsListRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<OfficialDto>();
            RequestedData.EntityId = ((int)Enums.Entities.RmOfficials);
            return await _officialService.GetOfficialsList(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> GetMayorsList(GetMayorsListRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<OfficialDto>();
            RequestedData.EntityId = ((int)Enums.Entities.Mayors);
            return await _officialService.GetOfficialsList(RequestedData);
        }

        [HttpPost]

        public async Task<OperationOutput> Activation(ActivationRecord RequestedData)
            => await _officialService.ModelAction(RequestedData.Adapt<OfficialDto>());

        [HttpPost]

        public async Task<OperationOutput> Delete(DeleteRecord RequestedData)
            => await _officialService.ModelAction(RequestedData.Adapt<OfficialDto>());

        [HttpPost]

        public async Task<OperationOutput> SortOrder(List<SortOrderRecord> RequestedData)
            => await _officialService.SortOrder(RequestedData.Adapt<List<OfficialDto>>());


        [AllowAnonymous]
        [HttpGet]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
              => _officialService.GetPathOfResource(fileName);
    }
}
