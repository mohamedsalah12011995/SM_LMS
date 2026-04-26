using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RM.Comments.Dtos;
using RM.Comments.Records;
using RM.Comments.Services;
using System;
using System.Collections.Generic;

namespace RM.Comments.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentsController
    {
        private readonly ICommentsService _commentsService;
        public CommentsController(ICommentsService commentsHandler)
        {
            _commentsService = commentsHandler;
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput InsertComment(InsertCommentRecord RequestedData)
        {
           return _commentsService.InsertComment(RequestedData.Adapt<Dtos.Comments>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput ApproveRejectComment(List<ApproveRejectCommentRecord> comments)
        {
            return _commentsService.ApproveRejectComment(comments.Adapt<List<Dtos.Comments>>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput AddReply(AddReplyRecord RequestedData)
        {
            return _commentsService.AddReply(RequestedData.Adapt<Dtos.Comments>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetAllComments(GetAllCommentsRecord RequestedData)
        {
            return await _commentsService.GetCommentsListAsync(RequestedData.Adapt<Dtos.Comments>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetUserEntities()
        {
            return _commentsService.GetUserEntities();
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task< OperationOutput> GetCommentsListByEntity(GetAllCommentsRecord RequestedData)
        {
            return await _commentsService.GetCommentsListByEntityAsync(RequestedData.Adapt<Dtos.Comments>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetCommentDetails(GetCommentDetailsRecord RequestedData)
        {
            return  _commentsService.GetCommentDetails(RequestedData.Adapt<Dtos.Comments>());
        }

    }
}
