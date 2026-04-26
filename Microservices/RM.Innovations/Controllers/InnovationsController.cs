
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using RM.Innovations.Dtos;
using System.IO;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using RM.Innovations.Services;
using RM.Innovations.Records;
using Mapster;
using RM.Core.CommonDtos;

namespace RM.Innovations.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InnovationsController : ControllerBase
    {
        IInnovationService _innovationService;
        ICommentsService _commentsService;
        IStatisticsService _statisticsService;
        public InnovationsController(IInnovationService innovationService, ICommentsService commentsService, IStatisticsService statisticsService)
        {
            _commentsService = commentsService;
            _innovationService = innovationService;
            _statisticsService = statisticsService;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> AddIdea940(AddIdea940Record RequestedData)
        {
            return await _innovationService.AddIdea940(RequestedData.Adapt<Dtos.Ideas>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> AddIdea(AddIdeaRecord RequestedData)
        {
            return await _innovationService.AddIdea(RequestedData.Adapt<Dtos.Ideas>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> AddPublicIdea(AddPublicIdeaRecord RequestedData)
        {
            return await _innovationService.AddPublicIdea(RequestedData.Adapt<Dtos.Ideas>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetUserIdeas()
        {
            return await _innovationService.GetUserIdeas();
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> GetIdeasList(GetIdeasListRecord RequestedData)
        {
            return await _innovationService.GetIdeasList(RequestedData.Adapt<Dtos.Ideas>());
        }

        
        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetPublicIdeasList(GetPublicIdeasListRecord RequestedData)
        {
            return await _innovationService.GetPublicIdeasList(RequestedData.Adapt<Dtos.Ideas>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetIdeasLookups()
        {
            return await _innovationService.GetIdeasLookups();
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetIdeasDetails(GetIdeasDetailsRecord RequestedData)
        {
            return await _innovationService.GetIdeasDetails(RequestedData.Adapt<Dtos.Ideas>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> SaveIdeaDetail(SaveIdeaDetailRecord RequestedData)
        {
           return await _innovationService.SaveIdeaDetail(RequestedData.Adapt<Dtos.Ideas>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> IdeaAction(IdeaActionRecord RequestedData)
        {
           return await _innovationService.IdeaAction(RequestedData.Adapt<Dtos.Ideas>());
        }

        //[HttpPost]
        //[Produces("application/json")]
        //public OperationOutput SendingEmail()
        //{
        //    return _innovationService.SendingEmail();
        //}

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> IdeaAgreement(IdeaAgreementRecord idea)
        {
            return await _innovationService.IdeaAgreement(idea.Adapt<Dtos.Ideas>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> IdeaChangeAgreement(IdeaChangeAgreementRecord idea)
        {
            return await _innovationService.IdeaChangeAgreement(idea.Adapt<Dtos.Ideas>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> IdeasStatisticCounter(IdeasStatisticCounterRecord idea)
        {
            return await _innovationService.IdeasStatisticCounter(idea.Adapt<Dtos.Ideas>());
        }
        
        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> InsertComment(InsertCommentRecord RequestedData)
        {
            return await _commentsService.InsertComment(RequestedData.Adapt<Dtos.IdeaComments>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> ApproveRejectComment(ApproveRejectCommentRecord RequestedData)
        {
            return await _commentsService.ApproveRejectComment(RequestedData.Adapt<Dtos.IdeaComments>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> AddReply(AddReplyRecord RequestedData)
        {
            return await _commentsService.AddReply(RequestedData.Adapt<Dtos.IdeaComments>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetCommentsList(GetCommentsListRecord RequestedData)
        {
           return await _commentsService.GetCommentsList(RequestedData.Adapt<Dtos.IdeaComments>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetCommentDetails(GetCommentDetailsRecord RequestedData)
        {
           return await _commentsService.GetCommentDetails(RequestedData.Adapt<Dtos.IdeaComments>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> DeleteComment(DeleteCommentRecord RequestedData)
        {
           return await _commentsService.DeleteComment(RequestedData.Adapt<Dtos.IdeaComments>());
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
        {
            return _innovationService.GetPathOfResource(fileName);
        }

        [HttpPost]
        [Produces("application/json")]
        public  IActionResult ExportXls(ExportRecord RequestedData)
        {
           var _data = _innovationService.Export(RequestedData.Adapt<Dtos.Ideas>());
           var _file = File(_data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Exported_{DateTime.Now}.xlsx");
           return Ok(_file.FileContents);      
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> TransferSuggestionToInnovation(TransferSuggestionToInnovationRecord RequestedData)
        {
           return await _innovationService.TransferSuggestionToInnovation(RequestedData.Adapt<Dtos.ContactUs>());
        }


        #region IdeasStatistics

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetLookUps(Dtos.Statistics RequestedData)
        {
            return await _statisticsService.GetLookUps(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetIdeasStatistics(Dtos.Statistics RequestedData)
        {
            return await _statisticsService.GetIdeasStatistics(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetIdeasStatisticsReport(Dtos.Statistics RequestedData)
        {
            return await _statisticsService.GetIdeasStatisticsReport(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SendEmailIdeasStatistics(Dtos.Statistics RequestedData)
        {
            return await _statisticsService.SendEmailIdeasStatistics(RequestedData);
        }


        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord RequestedData)
        {
            return await _statisticsService.CronJobSendReportsByEmail(RequestedData);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<IActionResult> ExportIdeasStatistics(Dtos.Statistics RequestedData)
        {
            return await _statisticsService.ExportIdeasStatistics(RequestedData);
        }
        #endregion

    }
}
