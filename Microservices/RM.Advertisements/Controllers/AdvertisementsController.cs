
using DocumentFormat.OpenXml.Drawing.Charts;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using RM.Advertisements.Dtos;
using RM.Advertisements.Records;
using RM.Advertisements.Services;
using RM.Core.Helpers;



namespace RM.Advertisements.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdvertisementsController 
    {
       
        private readonly IAdvertisementsService _advertisementsService;

        public AdvertisementsController(IAdvertisementsService advertisementsHandler)
        {
            _advertisementsService = advertisementsHandler;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetAdvertismentsList(GetAdvertismentsListRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<Dtos.Advertisements>();
            if (RequestedData.IsHomeSliderAd != true)
            {
                RequestedData.EntityId = (int)Core.Helpers.Enums.Entities.Advertisments;
                return await _advertisementsService.GetAdvertismentsList(RequestedData);
            }
            else
            {
                RequestedData.EntityId = (int)Core.Helpers.Enums.Entities.MainSlider;
                return await _advertisementsService.GetMainSliderList(RequestedData);
            }
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> GetAdvertismentsDetails(GetAdvertismentsDetailsRecord RequestedData)
        {
              return await _advertisementsService.GetAdvertismentDetails(RequestedData.Adapt<Dtos.Advertisements>());
        }
        //To Do
        [HttpPost("today")]
        [Produces("application/json")]
        public async Task<OperationOutput> GetTodayAdvertisements(GetTodayAdvertisementsRecord RequestedData)
        {
            return await _advertisementsService.GetTodayAdvertisementsAsync(RequestedData.Adapt<Dtos.Advertisements>());
        }
        //To Do
        [HttpPost("between-dates")]
        [Produces("application/json")]
        public async Task<OperationOutput> GetAdvertisementsBetweenDates(GetAdvertisementsBetweenDatesRecord RequestedData)
        {
            return await _advertisementsService.GetAdvertisementsBetweenDatesAsync(RequestedData.Adapt<Dtos.Advertisements>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveAdvertisment(SaveAdvertismentRecord RequestedData)
        {
            return await _advertisementsService.SaveAdvertisment(RequestedData.Adapt<Dtos.Advertisements>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput Activation(ActivationRecord RequestedData)
        {
              return _advertisementsService.ModelActions(RequestedData.Adapt<Dtos.Advertisements>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput Delete(DeleteRecord RequestedData)
        {
              return _advertisementsService.ModelActions(RequestedData.Adapt<Dtos.Advertisements>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput SortOrder(List<SortOrderRecord> RequestedData)
        {
             return _advertisementsService.SortOrder(RequestedData.Adapt<List<Dtos.Advertisements>>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetPlatformAdvertisments(GetPlatformAdvertismentsRecord RequestedData)
        {
             return await _advertisementsService.GetPlatformAdvertismentsList(RequestedData.Adapt<Dtos.Advertisements>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SavePlatformAdvertisment(SavePlatformAdvertismentRecord RequestedData)
        {
             return await _advertisementsService.SavePlatformAdvertisment(RequestedData.Adapt<Dtos.Advertisements>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetPlatformAdvertismentDetails(GetPlatformAdvertismentDetailsRecord RequestedData)
        {
             return await _advertisementsService.GetPlatformAdvertismentDetails(RequestedData.Adapt<Dtos.Advertisements>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetPlatformLookups()
        {
             return _advertisementsService.GetPlatformLookups();
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetReferences()
        {
             return _advertisementsService.GetReferences();
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
        {
            return _advertisementsService.GetPathOfResource(fileName);

        }

    }
}
