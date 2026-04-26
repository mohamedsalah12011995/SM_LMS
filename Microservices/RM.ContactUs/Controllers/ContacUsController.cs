
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.ContactUs.Dtos;
using RM.ContactUs.Records;
using RM.ContactUs.Services;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RM.ContactUs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ContactUsController 
    {
        private readonly IContactUsService _contactUsService;
        private readonly IFollowUpContactUsService _followUpContactUsHandlerHandler;
        private OperationOutput Result = new OperationOutput();
        private readonly IConfiguration _Configuration;
        private readonly IStatisticsService _statisticsService;   
        public ContactUsController(ILogger<OperationOutput> logger, IContactUsService contactUsHandler, IFollowUpContactUsService followUpContactUsHandlerHandler, IConfiguration configuration, IStatisticsService statisticsService)
        {
            _contactUsService = contactUsHandler;
            _followUpContactUsHandlerHandler = followUpContactUsHandlerHandler;
            _Configuration = configuration;
            _statisticsService = statisticsService;
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput SaveContactUs(SaveContactUsRecord RequestedData)
        {
            return _contactUsService.SaveContactUs(RequestedData.Adapt<Dtos.ContactUs>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput SaveContactMayor(SaveContactUsRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<Dtos.ContactUs>();
            RequestedData.EntityId = (int)Enums.Entities.ContactUs_Mayor;
            return _contactUsService.SaveContactUs(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput SaveContact940(SaveContactUsRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<Dtos.ContactUs>();
            RequestedData.EntityId = (int)Enums.Entities.ContactUs_940;
            return _contactUsService.SaveContactUs(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput SaveContactDeaf(SaveContactUsRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<Dtos.ContactUs>();
            RequestedData.EntityId = (int)Enums.Entities.ContactUs_Deaf;
            return _contactUsService.SaveContactUs(RequestedData);
        }


        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> GetContactUsList(GetContactUsListRecord RequestedData)
        {
            return await _contactUsService.GetContactUsList(RequestedData.Adapt<Dtos.ContactUs>());
        }


        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> GetContactUsDetails(GetContactUsDetailsRecord RequestedData)
        {
            return await _contactUsService.GetContactUsDetails(RequestedData.Adapt<Dtos.ContactUs>());
        }
      
        
        [HttpPost]
        [Produces("application/json")]
        public OperationOutput CpGetContactUsDetails(CpGetContactUsDetailsRecord RequestedData)
        {
            return _contactUsService.CpGetContactUsDetails(RequestedData.Adapt<Dtos.ContactUs>());
        }
     
        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetMayorComplainDetails(GetMayorComplainDetailsRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<Dtos.ContactUs>();
            RequestedData.EntityId = (int)Enums.Entities.ContactUs_Mayor;
            return await _contactUsService.GetContactUsDetails(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> Get940ComplainDetails(Get940ComplainDetailsRecord RequestedRecord)
        {
            var RequestedData = RequestedRecord.Adapt<Dtos.ContactUs>();
            RequestedData.EntityId = (int)Enums.Entities.ContactUs_940;
            return await _contactUsService.GetContactUsDetails(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> Get940Categories(Get940CategoriesRecord RequestedData)
        {
            return await _contactUsService.GetAmana940Categories(RequestedData.Adapt<Dtos.Amana940Category>());
        }


        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetContactListForManager(GetContactListForManagerRecord RequestedData)
        {
            return await _followUpContactUsHandlerHandler.GetContactListForManager(RequestedData.Adapt<Dtos.ContactUs>());
        }


        [HttpPost]
        [Produces("application/json")]
        public OperationOutput AddAction(AddActionRecord action)
        {
            return _followUpContactUsHandlerHandler.AddAction(action.Adapt<Dtos.Action>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetContactListForFollowUpUser(GetContactListForFollowUpUserRecord RequestedData)
        {
            return await _followUpContactUsHandlerHandler.GetContactListForFollowUpUser(RequestedData.Adapt<Dtos.ContactUs>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetFollowUpOfficerLookup()
        {
            return _followUpContactUsHandlerHandler.GetFollowUpOfficerLookup();
        }


        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetComplaintListForProccessorUser(GetComplaintListForProccessorUserRecord RequestedData)
        {
           return await _followUpContactUsHandlerHandler.GetComplaintListForProccessorUser(RequestedData.Adapt<Dtos.ContactUs>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetProccessorUserLookup()
        {
           return _followUpContactUsHandlerHandler.GetProccessorUserLookup();
        }


        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetContactActions(GetContactActionsRecord RequestedData)
        {
           return _followUpContactUsHandlerHandler.GetContactActions(RequestedData.Adapt<Dtos.ContactUs>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetContactDetails(GetContactDetailsRecord RequestedData)
        {
           return _contactUsService.GetContactDetails(RequestedData.Adapt<Dtos.ContactUs>());
        }


        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> AddEvaluation(AddEvaluationRecord RequestedData)
        {
           return await _followUpContactUsHandlerHandler.AddEvaluation(RequestedData.Adapt<Dtos.Feedback>());
        }


        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetContactListForQualityAssurance(GetContactListForQualityAssuranceRecord RequestedData)
        {
           return await _followUpContactUsHandlerHandler.GetContactListForQualityAssurance(RequestedData.Adapt<Dtos.ContactUs>());
        }
      
        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetContactFeedbacks(GetContactFeedbacksRecord RequestedData)
        {
           return _followUpContactUsHandlerHandler.GetContactFeedbacks(RequestedData.Adapt<Dtos.ContactUs>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetRegionRefernces()
        {
           return _contactUsService.GetRegionRefernces();
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
        {
            return _contactUsService.GetPathOfResource(fileName);
        }

        #region ComplaintsStatistics

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetLookUps(Dtos.Statistics RequestedData)
        {
            return await _statisticsService.GetLookUps(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetSuggestionsAndComplaintsStatistics(Dtos.Statistics RequestedData)
        {
            return await _statisticsService.GetSuggestionsAndComplaintsStatistics(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetSuggestionsAndComplaintsStatisticsReport(Dtos.Statistics RequestedData)
        {
            return await _statisticsService.GetSuggestionsAndComplaintsStatisticsReport(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SendEmailSuggestionsAndComplaintsStatistics(Dtos.Statistics RequestedData)
        {
            return await _statisticsService.SendEmailSuggestionsAndComplaintsStatistics(RequestedData);
        }


        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord RequestedData)
        {
            return await _statisticsService.CronJobSendReportsByEmail(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> ExportSuggestionsAndComplaintsStatistics(Dtos.Statistics RequestedData)
        {
            return await _statisticsService.ExportSuggestionsAndComplaintsStatistics(RequestedData);
        }
        #endregion


    }
}
