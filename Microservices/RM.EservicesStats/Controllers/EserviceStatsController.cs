using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RM.EservicesStats.Services;
using RM.EservicesStats.Dtos;
using System;

namespace RM.EservicesStats.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EserviceStatsController 
    {
        IEserviceStatsService EserviceStatsService;
        public EserviceStatsController(IEserviceStatsService _EserviceStatsService,IHttpContextAccessor httpContextAccessor,IConfiguration Configuration, ILogger<EserviceStatsController> logger)
        {
            EserviceStatsService = _EserviceStatsService;
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetTotalStatsYearly()
        {
            return EserviceStatsService.GetTotalStatsYearly();
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetBuildingStatsYearly()
        {
            return EserviceStatsService.GetBuildingStatsYearly();
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetBuildingStatsMonthly()
        {
            return EserviceStatsService.GetBuildingStatsMonthly();
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetMedicalStatsYearly()
        {
            return EserviceStatsService.GetMedicalStatsYearly();
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetMedicalStatsMonthly()
        {
           return EserviceStatsService.GetMedicalStatsMonthly();
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetTradingStatsYearly()
        {
           return EserviceStatsService.GetTradingStatsYearly();
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetTradingStatsMonthly()
        {
           return EserviceStatsService.GetTradingStatsMonthly();
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetSWPStatsYearly()
        {
           return EserviceStatsService.GetSWPStatsYearly();
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetSWPStatsMonthly()
        {
           return EserviceStatsService.GetSWPStatsMonthly();
        }


    }
}
