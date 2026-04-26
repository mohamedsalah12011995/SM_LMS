
using Mapster;
using Microsoft.AspNetCore.Mvc;
using RM.Lookups.Dtos;
using RM.Lookups.Records;
using RM.Lookups.Services;

namespace Lookups.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LookupsController 
    {
        private readonly ILookupsService _lookupsService;
        private readonly IRecommendionService _recommendionService;
        private readonly ICronSettingsService _cronSettingsService;

        public LookupsController(ILookupsService lookupsService, IRecommendionService recommendionService, ICronSettingsService cronSettingsService)
        {
            _lookupsService = lookupsService;
            _recommendionService = recommendionService;
            _cronSettingsService = cronSettingsService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetLookups(GetLookupsRecord RequestedData)
        {
           return await _lookupsService.GetLookups(RequestedData.Adapt<MajorLookups>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetLookupsList(GetLookupsListRecord RequestedData)
        {
           return await _lookupsService.GetLookupsList(RequestedData.Adapt<MajorLookupsType>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetLookupDetails(GetLookupDetailsRecord RequestedData)
        {
           return await _lookupsService.GetLookupDetails(RequestedData.Adapt<MajorLookupsType>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveLookup(SaveLookupRecord RequestedData)
        {
           return await _lookupsService.SaveLookup(RequestedData.Adapt<MajorLookupsType>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> Activation(ActivationRecord RequestedData)
        {
           return await _lookupsService.Activation(RequestedData.Adapt<MajorLookupsType>());
        }
     
        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> DeleteMajorLookup(DeleteMajorLookupRecord RequestedData)
        {
           return await _lookupsService.DeleteMajorLookup(RequestedData.Adapt<MajorLookups>());
        }


        #region SurveyRecommendations

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetRecommendationLookups(Recommendations RequestedData)
        {
            return await _recommendionService.GetLookups(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetAllRecommendation(Recommendations dto)
        {
            return await _recommendionService.GetAllAsync(dto);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetRecommendation(Recommendations dto)
        {
            return await _recommendionService.GetAsync(dto);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> AddRecommendation(Recommendations dto)
        {
            return await _recommendionService.AddAsync(dto);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> UpdateRecommendation(Recommendations dto)
        {
            return await _recommendionService.UpdateAsync(dto);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> DeleteRecommendation(Recommendations dto)
        {
            return await _recommendionService.DeleteAsync(dto);
        }

        #endregion



        #region CronSettings

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetCronSettingsLookups(CronSettings RequestedData)
        {
            return await _cronSettingsService.GetLookups(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetUsersLookup(CronSettings RequestedData)
        {
            return await _cronSettingsService.GetUsersLookup(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetAllCronSettings(CronSettings dto)
        {
            return await _cronSettingsService.GetAllAsync(dto);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveCronSettings(List<CronSettings> dto)
        {
            return await _cronSettingsService.SaveAsync(dto);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetCronSettings(CronSettings dto)
        {
            return await _cronSettingsService.GetAsync(dto);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> AddCronSettings(CronSettings dto)
        {
            return await _cronSettingsService.AddAsync(dto);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> UpdateCronSettings(CronSettings dto)
        {
            return await _cronSettingsService.UpdateAsync(dto);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> DeleteCronSettings(CronSettings dto)
        {
            return await _cronSettingsService.DeleteAsync(dto);
        }

        #endregion

    }
}
