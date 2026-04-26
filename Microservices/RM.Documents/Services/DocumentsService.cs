
using Mapster;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.Core.Services;
using RM.Documents.Dtos;
using RM.Documents.UnitOfWorks;
using RM.Models;
using static RM.Documents.Dtos.OperationOutput;

namespace RM.Documents.Services
{
    public class DocumentsService : BaseService, IDocumentsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public List<string> notAllowedExtensions;
        public DocumentsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            notAllowedExtensions = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("NotAllowedUploadExtensions").Value.Split(',').ToList();
        }

        public OperationOutput GetDocumentLookups(Dtos.Documents RequestedData, Enums.DocumentsType DocumentType)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Item = _unitOfWork.Documents.GetAll().Include(x => x.InverseParent)
                .Where(x => x.IsDeleted == false && x.ReferenceId == RequestedData.ReferenceId && x.ParentId == null && x.IsActive == true)
                .AsNoTracking().Adapt<List<Dtos.Documents>>(Dtos.Documents.SelectConfig(DocumentsGetPath, false));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.DocumentsEntity, Item));

        }
        public async Task<OperationOutput> GetDocumentList(Dtos.Documents RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (IsPortal || RequestUserRole == Enums.UsersRoles.NormalUser)
                RequestedData.IsActive = true;

            var filteration = RequestedData.DocumentsFilteration();
            var Documents = await _unitOfWork.Documents.FindAllByPaginationAsync(filteration, RequestedData.Pagination, DefaultPaginationCount, x => x.Id,
                OrderBy.Descending, x => x.CreatedByNavigation);

            var DocumentsDto = Documents.Data.Adapt<List<Dtos.Documents>>(Dtos.Documents.SelectConfig(DocumentsGetPath, false));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, RequestedData.ParentId.HasValue ? (int)Enums.Entities.Versions : (int)Enums.Entities.Magazine, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.DocumentsEntity, DocumentsDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.Departments)),
                   new OutputDictionary(OperationOutputKeys.Pagination, Documents.Pagination));
        }
        public async Task<OperationOutput> GetDocumentDetails(Dtos.Documents RequestedData)
        {
            return await GetDocumentDetails(RequestedData.Id.Value);
        }
        public async Task<OperationOutput> GetDocumentDetails(int Id)
        {
            Dtos.Documents ItemDto = new Dtos.Documents();
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Item = _unitOfWork.Documents.GetAll().Include(x => x.Parent)
                       .Where(x => x.Id == Id && x.IsDeleted != true)
                       .AsNoTracking().FirstOrDefault();

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            Item.Adapt(ItemDto, Dtos.Documents.SelectConfig(DocumentsGetPath, false));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, Item.ReferenceId, (int)Enums.Entities.Versions, Item.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.DocumentsEntity, ItemDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, ItemDto.entityId));
        }
        public async Task<OperationOutput> SaveDocumentsCategory(Dtos.Documents RequestedData)
        {
            Document DbItem = new Document();
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestedData.Id.HasValue)
            {
                DbItem = _unitOfWork.Documents.GetById(RequestedData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem, RequestedData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.Documents.Update(DbItem);
            }
            else
            {
                RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id));
                _unitOfWork.Documents.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.DocumentsEntity, DbItem.Id));
        }
        public async Task<OperationOutput> SaveDocuments(Dtos.Documents RequestedData)
        {
            Document DbItem = new Document();
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            RequestedData.IsFinalRoot = true;

            if (!Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.UrlBase64))
                RequestedData.Url = Files.SaveBase64FileToServer(Strings.GenerateGUID() + ".pdf", RequestedData.UrlBase64, DocumentsSavePath);

            if (RequestedData.Id.HasValue)
            {
                DbItem = _unitOfWork.Documents.GetById(RequestedData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem, RequestedData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.Documents.Update(DbItem);
            }
            else
            {
                RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id));
                _unitOfWork.Documents.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();
            return await GetDocumentDetails(DbItem.Id);
        }
        public OperationOutput SaveEditorDocs(Dtos.Documents RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var fileName = Files.SaveBase64FileToServer(RequestedData.FileName, RequestedData.UrlBase64, EditorsDocsSavePath);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.Url, EditorsDocsGetPath + "/" + fileName));
        }

        public OperationOutput ModelActions(Dtos.Documents RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.Documents.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            DbItem.IsActive = RequestedData.IsActive.HasValue ? RequestedData.IsActive.Value : DbItem.IsActive;
            DbItem.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : DbItem.IsDeleted;
            if (RequestedData.IsDeleted.HasValue && RequestedData.IsDeleted.Value == true)
            {
                DbItem.DeletedBy = RequestOwner.Id;
                DbItem.DeletedDate = TransactionDate;
            }
            if (RequestedData.IsActive.HasValue && RequestedData.IsActive.Value == true)
            {
                DbItem.ActivatedBy = RequestOwner.Id;
                DbItem.ActivatedDate = TransactionDate;
            }
            DbItem.UpdatedBy = RequestOwner.Id;
            DbItem.UpdatedDate = DateTime.Now;

            _unitOfWork.Documents.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> UploadFileAsync(Stream fileStream, string contentType)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var fileCount = 0;
            long totalSizeInBytes = 0;

            var boundary = GetBoundary(MediaTypeHeaderValue.Parse(contentType));
            var multipartReader = new MultipartReader(boundary, fileStream);

            var filePaths = new List<string>();
            var notUploadedFiles = new List<string>();

            var section = await multipartReader.ReadNextSectionAsync();

            while (section != null)
            {
                var fileSection = section.AsFileSection();
                if (fileSection != null)
                {

                    totalSizeInBytes += await SaveFileAsync(EditorsDocsSavePath, fileSection, filePaths, notUploadedFiles);
                    fileCount++;
                }

                section = await multipartReader.ReadNextSectionAsync();
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.Urls, filePaths));
        }
        public async Task<OperationOutput> GetGuideDocumentByEntityId(Dtos.GuideDocument RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            var documentUrl = await _unitOfWork.Documents.GetAll().AsNoTracking()
                .Where(x => x.EntityId == RequestedData._entityId
                    && x.TypeId == RequestedData._typeId)
                .Select(x => new { Url = DocumentsGetPath + "/" + x.Url }).FirstOrDefaultAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.DocumentsEntity, documentUrl != null ? documentUrl.Url : string.Empty));


        }



        #region Utilis

        private async Task<long> SaveFileAsync(string savePath, FileMultipartSection fileSection, IList<string> filePaths, IList<string> notUploadedFiles)
        {
            var extension = Path.GetExtension(fileSection.FileName);

            var FileName = ClearSpecial(fileSection.FileName.Replace(extension, string.Empty)) + extension;


            if (notAllowedExtensions.Contains(extension))
            {
                notUploadedFiles.Add(FileName);
                return 0;
            }

            string nPath = System.IO.Path.Combine(savePath, FileName);

            if (File.Exists(@nPath))
                FileName = TransactionDate.Millisecond.ToString() + "_" + FileName;

            await using var stream = new FileStream(Path.Combine(savePath, FileName), FileMode.Create, FileAccess.Write, FileShare.None, 1024);
            await fileSection.FileStream?.CopyToAsync(stream);

            var Url = EditorsDocsGetPath + "/" + FileName;
            var UrlInnerGate = EditorsDocsGetPathInnerGate + "/" + FileName;

            filePaths.Add(Url);
            filePaths.Add(UrlInnerGate);

            return fileSection.FileStream.Length;
        }


        private string ConvertSizeToString(long bytes)
        {
            var fileSize = new decimal(bytes);
            var kilobyte = new decimal(1024);
            var megabyte = new decimal(1024 * 1024);
            var gigabyte = new decimal(1024 * 1024 * 1024);

            return fileSize switch
            {
                _ when fileSize < kilobyte => "Less then 1KB",
                _ when fileSize < megabyte =>
                    $"{Math.Round(fileSize / kilobyte, fileSize < 10 * kilobyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}KB",
                _ when fileSize < gigabyte =>
                    $"{Math.Round(fileSize / megabyte, fileSize < 10 * megabyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}MB",
                _ when fileSize >= gigabyte =>
                    $"{Math.Round(fileSize / gigabyte, fileSize < 10 * gigabyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}GB",
                _ => "n/a"
            };
        }

        private string GetBoundary(MediaTypeHeaderValue contentType)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            return boundary;
        }


        public string ClearSpecial(string str)
        {
            return str.Trim().Replace("_", "").Replace("-", "").Replace(" ", "").Replace("\\", "").Replace("/", "").Replace(".", "").Replace("$", "").ToLower();
        }



        #endregion
    }
}
