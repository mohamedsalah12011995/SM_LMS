using Mapster;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RM.FAQs.Dtos;
using RM.FAQs.Records;
using RM.FAQs.Services;

namespace RM.FAQs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FAQsController : ControllerBase
    {

        private readonly IFAQService _FAQService;
        public FAQsController(IFAQService FAQService)
        {
            _FAQService = FAQService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetFAQList(GetFAQListRecord RequestedData)
        {
            return await _FAQService.GetFAQList(RequestedData.Adapt<Dtos.FAQ>());
        }
        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetFAQByID(GetFAQByIDRecord RequestedData)
        {
            return await _FAQService.GetFAQDetails(RequestedData.Adapt<Dtos.FAQ>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveFAQ(SaveFAQRecord RequestedData)
        {
            return await _FAQService.SaveFAQ(RequestedData.Adapt<Dtos.FAQ>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> Delete(DeleteRecord RequestedData)
        {
            return await _FAQService.FAQModelActions(RequestedData.Adapt<Dtos.FAQ>());
        }



    }
}
