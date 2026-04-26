
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.Core.Helpers;
using RM.Core.Utilities;
using RM.Documents.Dtos;
using RM.Documents.Records;
using RM.Documents.Services;

namespace RM.Documents.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {

        private readonly IDocumentsService _documentsService;
        public DocumentsController(IDocumentsService documentsService)
        {
            _documentsService = documentsService;
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput GetVersionsDocumentsLookups(GetVersionsDocumentsLookupsRecord RequestedData)
        {
            return _documentsService.GetDocumentLookups(RequestedData.Adapt<Dtos.Documents>(), Enums.DocumentsType.Versions);
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetDocumentVersion(GetDocumentVersionRecord RequestedData)
        {
            return await _documentsService.GetDocumentList(RequestedData.Adapt<Dtos.Documents>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveDocuments(SaveDocumentsRecord RequestedData)
        {
            return await _documentsService.SaveDocuments(RequestedData.Adapt<Dtos.Documents>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> SaveDocumentsCategory(SaveDocumentsCategoryRecord RequestedData)
        {
            return await _documentsService.SaveDocumentsCategory(RequestedData.Adapt<Dtos.Documents>());
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GetDocumentDetails(GetDocumentDetailsRecord RequestedData)
        {
            return await _documentsService.GetDocumentDetails(RequestedData.Adapt<Dtos.Documents>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput Activation(ActivationRecord RequestedData)
        {
            return _documentsService.ModelActions(RequestedData.Adapt<Dtos.Documents>());
        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput Delete(DeleteRecord RequestedData)
        {
            return _documentsService.ModelActions(RequestedData.Adapt<Dtos.Documents>());

        }

        [HttpPost]
        [Produces("application/json")]
        public OperationOutput SaveEditorDocs(SaveEditorDocsRecord RequestedData)
        {
            return _documentsService.SaveEditorDocs(RequestedData.Adapt<Dtos.Documents>());
        }

        [HttpPost]
        public async Task<OperationOutput> GetGuideDocumentByEntityId(Dtos.GuideDocument RequestedData)
        => await _documentsService.GetGuideDocumentByEntityId(RequestedData);


        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [MultipartFormData]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> UploadFile()
        {
            var fileUploadSummary = await _documentsService.UploadFileAsync(HttpContext.Request.Body, Request.ContentType);
            return CreatedAtAction(nameof(UploadFile), fileUploadSummary);
        }

        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
        {
            return _documentsService.GetPathOfResource(fileName);
        }
    }
}
