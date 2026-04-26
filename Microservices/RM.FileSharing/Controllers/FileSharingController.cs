using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RM.FileSharing.Dtos;
using RM.FileSharing.Services;
using System.Reflection;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using RM.FileSharing.Utilities;
using System.Collections.Generic;
using RM.FileSharing;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.StaticFiles;
using RM.FileSharing.Records;
using Mapster;

namespace FileSharing.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileSharingController : ControllerBase
    {
        private readonly IFileSharingService _FileSharingService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public FileSharingController(IFileSharingService FileSharingHandler)
        {
            _FileSharingService = FileSharingHandler;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetDirectory(GetDirectoryRecord RequestedData)
        {
            return await _FileSharingService.GetDirectory(RequestedData.Adapt<FileSharingInfo>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetGroups(GetGroupsRecord RequestedData)
        {
            return _FileSharingService.GetGroups(RequestedData.Adapt<FileSharingInfo>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetGroupMembers(GetGroupMembersRecord RequestedData)
        {
            return _FileSharingService.GetGroupMembers(RequestedData.Adapt<FileSharingInfo>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput UserOrGroupSearch(UserOrGroupSearchRecord RequestedData)
        {
            return _FileSharingService.UserOrGroupSearch(RequestedData.Adapt<FileSharingInfo>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetDirFileAccessRules(GetDirFileAccessRulesRecord RequestedData)
        {
            return _FileSharingService.GetDirFileAccessRules(RequestedData.Adapt<FileSharingInfo>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput ChangeDirFileAccessRules(ChangeDirFileAccessRulesRecord RequestedData)
        {
            return _FileSharingService.ChangeDirFileAccessRules(RequestedData.Adapt<FileSharingInfo>());
        }


        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit =500000000)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [MultipartFormData]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> UploadFile()
        {
            var fileUploadSummary = await _FileSharingService.UploadFileAsync(HttpContext.Request.Body, Request.ContentType);

            return CreatedAtAction(nameof(UploadFile), fileUploadSummary);
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput CreateDirectory(CreateDirectoryRecord RequestedData)
        {
            return _FileSharingService.CreateDirectory(RequestedData.Adapt<FileSharingInfo>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput DeleteFileOrDirectory(List<DeleteFileOrDirectoryRecord> RequestedData)
        {
            return _FileSharingService.DeleteFileOrDirectory(RequestedData.Adapt<List<FileSharingInfo>>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput RenameFileOrDirectory(RenameFileOrDirectoryRecord RequestedData)
        {
            return _FileSharingService.RenameFileOrDirectory(RequestedData.Adapt<FileSharingInfo>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput CopyFileOrDirectory(CopyFileOrDirectoryRecord RequestedData)
        {
            return _FileSharingService.CopyFileOrDirectory(RequestedData.Adapt<FilesSharingInfo>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput MoveFileOrDirectory(MoveFileOrDirectoryRecord RequestedData)
        {
            return _FileSharingService.MoveFileOrDirectory(RequestedData.Adapt<FilesSharingInfo>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetProccessStatus(GetProccessStatusRecord RequestedData)
        {
            return _FileSharingService.GetProccessStatus(RequestedData.Adapt<FileSharingInfo>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput CompressFiles(CompressFilesRecord RequestedData)
        {
            return _FileSharingService.CompressFiles(RequestedData.Adapt<FilesSharingInfo>());
        }

        [HttpPost]
        [Produces("application/json")]
        public IActionResult DownloadFile(DownloadFileRecord RequestedData)
        {
            string path = RequestedData.Path.Replace("//", "/");
            if (!System.IO.File.Exists(path))
                return NotFound();

            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Path.GetFileName(path),
            };

            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            var contentTypeProvider = new FileExtensionContentTypeProvider();
            if (!contentTypeProvider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            Response.Headers.Add("Content-Type", contentType);

            return new FileStreamResult(fileStream, contentType);
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetSearchResult(GetSearchResultRecord RequestedData)
        {
            return _FileSharingService.GetSearchResult(RequestedData.Adapt<FileSharingInfo>());
        }

    }
}
