using RM.Core.Helpers;
using RM.Core.Services;
using RM.Documents.Dtos;
namespace RM.Documents.Services
{
    public interface IDocumentsService : IBaseService
    {
        OperationOutput GetDocumentLookups(Dtos.Documents RequestedData, Enums.DocumentsType DocumentType);
        Task<OperationOutput> GetDocumentList(Dtos.Documents RequestedData);
        Task<OperationOutput> GetDocumentDetails(Dtos.Documents RequestedData);
        Task<OperationOutput> SaveDocumentsCategory(Dtos.Documents RequestedData);
        Task<OperationOutput> SaveDocuments(Dtos.Documents RequestedData);
        OperationOutput SaveEditorDocs(Dtos.Documents RequestedData);
        OperationOutput ModelActions(Dtos.Documents RequestedData);
        Task<OperationOutput> UploadFileAsync(Stream fileStream, string contentType);
        Task<OperationOutput> GetGuideDocumentByEntityId(Dtos.GuideDocument RequestedData);

    }
}
