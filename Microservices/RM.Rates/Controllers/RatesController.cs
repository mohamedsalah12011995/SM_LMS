using Microsoft.AspNetCore.Mvc;
using RM.Rates.Dtos;
using RM.Rates.Services;

namespace RM.Rates.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RatesController : ControllerBase
    {

        private readonly IRateService _ratesService;
        public RatesController(IRateService ratesService)
              => _ratesService = ratesService;



        [HttpPost]
        public async Task<OperationOutput> InsertRates(Dtos.Rates RequestedData)

            => await _ratesService.InsertRates(RequestedData);


    }

}
