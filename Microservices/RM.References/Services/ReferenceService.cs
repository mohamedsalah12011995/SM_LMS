using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.References.Dtos;
using RM.References.UnitOfWorks;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static RM.References.Dtos.OperationOutput;

namespace RM.References.Services
{

    public class ReferenceService : BaseService, IReferenceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReferenceService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<OperationOutput> GetLookups(Dtos.References RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var references = await _unitOfWork.Menu.GetAll().Where(x => x.ReferenceId == RequestedData.Id
                             && x.ParentId == null).AsNoTracking().ToListAsync();

            var referenceDto = references.Adapt<List<Dtos.References>>(Dtos.References.SelectConfig(ImagesGetPath));

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.References, referenceDto));
        }
        public OperationOutput GetMuniicipalitiesType()
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<ReferencesMajor> referencesMajors = new List<ReferencesMajor>
            {
              new ReferencesMajor() { Id = (int)Enums.ReferenceMajor.MainMnicipalities, NameAr = "البلديات الداخلية", NameEn = "Main Municipalities" },
              new ReferencesMajor() { Id = (int)Enums.ReferenceMajor.SubMunicipalities, NameAr = "البلديات الخارجية", NameEn = "Sub Municipalities" }
            };

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ReferencesMajorEntity, referencesMajors));

        }
        public async Task<OperationOutput> GetMainMenu(Dtos.References RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var menu = await _unitOfWork.Menu.GetAll(x => x.ReferenceId == RequestedData.ReferenceId
                                                     && x.ParentId == null && x.EntityId == null,
                                                     s => (int)s.MenuOrder, OrderBy.Ascending,
                                                     c => c.InverseParent).ToListAsync();


            var menuDto = menu.Adapt<List<Menu>>(Menu.SelectConfig());

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                 new OutputDictionary(OperationOutputKeys.MenuEntity, menuDto));
        }

        public async Task<OperationOutput> GetMenu(Menu RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            var menu = await _unitOfWork.Menu.GetAll(x => x.EntityId == RequestedData.EntityId
                                && x.ReferenceId == RequestedData.ReferenceId
                                && x.ParentId == null).Select(x => new Models.Menu
                                {
                                    Id = x.Id,
                                    ArticleId = x.ArticleId,
                                    MenuOrder = x.MenuOrder,
                                    NameAr = x.NameAr,
                                    NameEn = x.NameEn,
                                    Url = x.Url,
                                }).ToListAsync();

            var menuDto = menu.Adapt<List<Menu>>(Menu.SelectConfig());

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
              new OutputDictionary(OperationOutputKeys.MenuEntity, menuDto));
        }

        public OperationOutput DecryptString(string Value)
        {
            var decryptValue = Cryptography.AES.Decrypt(Value) ?? string.Empty;

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.ReferencesEntity, decryptValue));
        }
        public OperationOutput EncryptString(string Value)
        {
            var encryptValue = Cryptography.AES.Encrypt(Value);

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.ReferencesEntity, encryptValue));
        }

        public async Task<OperationOutput> GetReferencesList(Enums.ReferenceMajor ReferenceType, Dtos.References RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            int entityId = 0;

            entityId = CheckReferenceMajorTypeAndSetEntityId(ReferenceType, RequestedData, entityId);

            var references = await _unitOfWork.References.FindAllByPagination(RequestedData.Filteration(),
                              RequestedData.Pagination, DefaultPaginationCount, x => x.CreatedDate,
                              OrderBy.Descending, x => x.ReferenceContents, c => c.CreatedByNavigation, c => c.UpdatedByNavigation);

            var referencesDto = references.Data.Adapt<List<Dtos.References>>(Dtos.References.SelectConfig(ImagesGetPath));

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
              new OutputDictionary(OperationOutputKeys.ReferencesEntity, referencesDto),
              new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt(entityId)),
              new OutputDictionary(OperationOutputKeys.Pagination, references.Pagination));
        }

        #region HELPER METHOD TO GetReferencesList
        private static int CheckReferenceMajorTypeAndSetEntityId(Enums.ReferenceMajor ReferenceType, Dtos.References RequestedData, int EntityId)
        {
            switch (ReferenceType)
            {
                case Enums.ReferenceMajor.Departments:
                    EntityId = (int)Enums.Entities.Departments;
                    RequestedData.SearchInReferences.Add((int)Enums.ReferenceMajor.Departments);
                    break;
                case Enums.ReferenceMajor.Agencies:
                    EntityId = (int)Enums.Entities.Agencies;
                    RequestedData.SearchInReferences.Add((int)Enums.ReferenceMajor.Agencies);
                    break;
                case Enums.ReferenceMajor.SubMunicipalities:
                case Enums.ReferenceMajor.MainMnicipalities:
                    EntityId = (int)Enums.Entities.Municipalities;
                    RequestedData.SearchInReferences.Add((int)Enums.ReferenceMajor.SubMunicipalities);
                    RequestedData.SearchInReferences.Add((int)Enums.ReferenceMajor.MainMnicipalities);
                    break;
            }

            return EntityId;
        }

        #endregion

        public async Task<OperationOutput> GetReferencesDetails(Dtos.References RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var reference = await _unitOfWork.References.GetAll().Include(x => x.ReferenceContents)
                                 .Include(x => x.InverseParent).AsNoTracking()
                                 .FirstOrDefaultAsync(x => x.Id == RequestedData.Id);

            var referenceDto = reference.Adapt<Dtos.References>(Dtos.References.SelectConfig(ImagesGetPath));

            int entityId = GetEntityIdBasedOnRefernceMajor(referenceDto);

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.ReferencesEntity, referenceDto),
            new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt(entityId)));

        }

        #region HELPER METHOD FOR GetReferencesDetails
        private static int GetEntityIdBasedOnRefernceMajor(Dtos.References referenceDto)
        {
            if (referenceDto != null)
            {

                if (referenceDto.ReferencesMajorId == (int)Enums.ReferenceMajor.MainMnicipalities
                || referenceDto.ReferencesMajorId == (int)Enums.ReferenceMajor.SubMunicipalities)

                    return (int)Enums.Entities.Municipalities;

                else if (referenceDto.ReferencesMajorId == (int)Enums.ReferenceMajor.Departments)

                    return (int)Enums.Entities.Departments;
            }

            return 0;
        }
        #endregion


        public async Task<OperationOutput> GetReferenceContent(Dtos.References RequestedData)
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var referenceContent = await _unitOfWork.ReferenceContent.FindAsync(x => x.ReferenceId == RequestedData.Id.Value);
            var referenceContentDto = referenceContent.Adapt<ReferenceContent>(ReferenceContent.GetCustomMapping(ImagesGetPath));

            return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.ReferencesEntity, referenceContentDto));
        }

        public async Task<OperationOutput> SaveReferenceDetails(ReferenceContent RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var reference = await _unitOfWork.References.GetAll().Include(x => x.ReferenceContents).AsNoTracking().FirstOrDefaultAsync(x => x.Id == RequestedData.ReferenceId);

            var contentReference = reference.ReferenceContents != null ? reference.ReferenceContents.FirstOrDefault() : null;

            if (contentReference == null)
            {
                _unitOfWork.ReferenceContent.Add(RequestedData.Adapt(contentReference,
                RequestedData.AddConfig(RequestedData.ChiefImageBase64, ImagesSavePath, reference.Id)));
            }
            else _unitOfWork.ReferenceContent.Update(RequestedData.Adapt(contentReference));

            reference.UpdatedBy = RequestOwner.Id;
            reference.UpdatedDate = TransactionDate;

            _unitOfWork.References.Update(reference);
            _unitOfWork.Complete();

            var referenceDto = new Dtos.References() { Id = contentReference.ReferenceId };
            return await GetReferencesDetails(referenceDto);

        }

        public OperationOutput SearchAsText(SearchEngine.Request RequestedData)
        {
            List<Models.SearchEngine> _Item;
            OperationOutput Result = new OperationOutput();
            string ProcedureParameters = string.Empty;

            string ProcedureName = "Sp_SearchEngine";
            var parameters = new[] {
            new SqlParameter("@SearchWord", SqlDbType.NVarChar) {Size=int.MaxValue, Direction = ParameterDirection.Input, Value = RequestedData.SearchWord },
            new SqlParameter("@ReferenceId", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = RequestedData._referenceId.HasValue?RequestedData._referenceId:DBNull.Value },
            new SqlParameter("@FromDate", SqlDbType.NVarChar) {Size=int.MaxValue, Direction = ParameterDirection.Input, Value = RequestedData.FromDate.HasValue?RequestedData.FromDate:DBNull.Value },
            new SqlParameter("@ToDate", SqlDbType.NVarChar) { Size=int.MaxValue,Direction = ParameterDirection.Input, Value = RequestedData.ToDate.HasValue? RequestedData.ToDate:DBNull.Value },
            new SqlParameter("@TargetedEntities", SqlDbType.NVarChar) {Size=int.MaxValue, Direction = ParameterDirection.Input, Value = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData._targetedEntities)?RequestedData._targetedEntities:DBNull.Value },

            };

            ProcedureParameters = " @SearchWord,@ReferenceId,@FromDate,@ToDate,@TargetedEntities";

            List<Tuple<string, object>> Params = new List<Tuple<string, object>>();
            Params.Add(new Tuple<string, object>("SearchWord", RequestedData.SearchWord));
            Params.Add(new Tuple<string, object>("ReferenceId", RequestedData._referenceId.HasValue ? RequestedData._referenceId : DBNull.Value));
            Params.Add(new Tuple<string, object>("FromDate", RequestedData.FromDate.HasValue ? RequestedData.FromDate : DBNull.Value));
            Params.Add(new Tuple<string, object>("ToDate", RequestedData.ToDate.HasValue ? RequestedData.ToDate : DBNull.Value));
            Params.Add(new Tuple<string, object>("TargetedEntities", !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData._targetedEntities) ? RequestedData._targetedEntities : DBNull.Value));

            _Item = _unitOfWork.sp_SearchEngine_Result.QueryProcedure(ProcedureName, Params).ToList().Select(x => new Models.SearchEngine()
            {
                Url = x.Url + "/" + Cryptography.AES.Encrypt(x.Id),
                Id = x.Id,
                TitleAr = x.TitleAr,
                TitleEn = x.TitleEn,
                BriefeContentAr = x.BriefeContentAr,
                BriefeContentEn = x.BriefeContentEn,
                EntityId = x.EntityId,
                Date = x.Date,
                SearchNameAr = x.SearchNameAr,
                SearchNameEn = x.SearchNameEn,
            }).ToList();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.SearchResultEntity, _Item));
        }

        public async Task<OperationOutput> GetSearchableEntities()
        {
            if (RequestOwner is null)
                return OperationOutput.GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var entities = await _unitOfWork.Entity.GetAll().Where(x => x.Searchable == true)
                .Select(x => new Models.Entity()
                {
                    Id = x.Id,
                    NameAr = x.NameAr,
                    NameEn = x.NameEn
                }).AsNoTracking().ToListAsync();

            var entitiesDto = entities.Adapt<List<Entity>>(Entity.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.ReferencesEntity, entitiesDto));
        }

        public async Task<OperationOutput> GetReferenceMajorList(ReferencesMajor RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var referencesMajors = await _unitOfWork.ReferencesMajor.FindAllByPagination(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Ascending
                                  , x => x.CreatedByNavigation, x => x.UpdatedByNavigation);

            var referencesMajorsDto = referencesMajors.Data.Adapt<List<ReferencesMajor>>(ReferencesMajor.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.ReferencesMajors, referencesMajorsDto),
             new OutputDictionary(OperationOutputKeys.Pagination, referencesMajors.Pagination));

        }

        public async Task<OperationOutput> GetMajorReferenceLookup()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var referencesMajors = await _unitOfWork.ReferencesMajor.GetAll().Where(x => x.IsDeleted != true)
                 .Select(x => new Models.ReferencesMajor()
                 {
                     Id = x.Id,
                     NameAr = x.NameAr,
                     NameEn = x.NameEn,
                 }).AsNoTracking().ToListAsync();

            var referencesMajorsDto = referencesMajors.Adapt<List<ReferencesMajor>>(ReferencesMajor.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                  new OutputDictionary(OperationOutputKeys.ReferencesMajors, referencesMajorsDto));
        }

        public async Task<OperationOutput> GetReferenceMajorDetails(ReferencesMajor RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var referencesMajor = await _unitOfWork.ReferencesMajor.GetByIdAsync(RequestedData.Id.Value);
            var referencesMajorDto = referencesMajor.Adapt<ReferencesMajor>(ReferencesMajor.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                  new OutputDictionary(OperationOutputKeys.ReferencesMajors, referencesMajorDto));
        }

        public async Task<OperationOutput> SaveReferenceMajor(ReferencesMajor RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.ReferencesMajor referencesMajor = null;

            if (RequestedData.Id.HasValue)
            {
                referencesMajor = await _unitOfWork.ReferencesMajor.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.Id == RequestedData.Id.Value);
                if (referencesMajor == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                _unitOfWork.ReferencesMajor.Update(RequestedData.Adapt(referencesMajor, RequestedData.UpdateConfig(RequestOwner.Id)));
            }
            else
            {
                referencesMajor = new Models.ReferencesMajor();
                _unitOfWork.ReferencesMajor.Add(RequestedData.Adapt(referencesMajor, RequestedData.AddConfig(RequestOwner.Id)));
            }

            await _unitOfWork.CompleteAsync();
            RequestedData.Id = referencesMajor.Id;

            return await GetReferenceMajorDetails(RequestedData);
        }
        public async Task<OperationOutput> ModelActionsReferenceMajor(ReferencesMajor RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var referencesMajor = await _unitOfWork.ReferencesMajor.GetAll().AsNoTracking().FirstOrDefaultAsync(m => m.Id == RequestedData.Id.Value);

            if (referencesMajor == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            referencesMajor.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : referencesMajor.IsDeleted;

            if (RequestedData.IsDeleted.HasValue && RequestedData.IsDeleted.Value == true)

                _unitOfWork.ReferencesMajor.Update(RequestedData.Adapt(referencesMajor, RequestedData.DeleteConfig(RequestOwner.Id)));

            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> GetReferencesByFiltrations(Dtos.References RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var references = await _unitOfWork.References.FindAllByPagination(RequestedData.Filteration(),
                RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Ascending, x => x.CreatedByNavigation, x => x.UpdatedByNavigation);

            var referencesDto = references.Data.Adapt<List<Dtos.References>>(Dtos.References.SelectConfig(ImagesGetPath));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.ReferencesEntity, referencesDto),
             new OutputDictionary(OperationOutputKeys.Pagination, references.Pagination));
        }

        public async Task<OperationOutput> GetReference(Dtos.References RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var reference = await _unitOfWork.References.GetAll().AsNoTracking().FirstOrDefaultAsync(r => r.Id == RequestedData.Id.Value);

            if (reference == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var referenceDto = reference.Adapt<Dtos.References>(Dtos.References.SelectConfig(ImagesGetPath));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.ReferencesEntity, referenceDto));
        }

        public async Task<OperationOutput> SaveReference(Dtos.References RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.Reference references = null;

            if (RequestedData.Id.HasValue)
            {
                references = await _unitOfWork.References.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.Id == RequestedData.Id.Value);
                if (references == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                _unitOfWork.References.Update(RequestedData.Adapt(references, RequestedData.UpdateConfig(RequestOwner.Id)));
            }
            else
            {
                references = new Models.Reference();
                _unitOfWork.References.Add(RequestedData.Adapt(references, RequestedData.AddConfig(RequestOwner.Id)));
            }

            await _unitOfWork.CompleteAsync();
            RequestedData.Id = references.Id;

            return await GetReference(RequestedData);
        }

        public async Task<OperationOutput> ModelActionsReferences(Dtos.References RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var reference = await _unitOfWork.References.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.Id == RequestedData.Id.Value);

            if (reference == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            reference.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : reference.IsDeleted;

            if (RequestedData.IsDeleted.HasValue && RequestedData.IsDeleted.Value == true)
                _unitOfWork.References.Update(RequestedData.Adapt(reference, RequestedData.DeleteConfig(RequestOwner.Id)));

            await _unitOfWork.CompleteAsync();
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> GetReferencesTreeByMajorId(Dtos.References RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var references = await _unitOfWork.References.GetAll(x => x.ReferencesMajorId == RequestedData.ReferencesMajorId
                            && x.IsDeleted != true).AsNoTracking().ToListAsync();

            var referencesTree = GenerateReferenceTree(references, null).Adapt<List<Dtos.ReferencesTree>>(Dtos.ReferencesTree.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.ReferencesEntity, referencesTree));
        }

        public async Task<OperationOutput> GetReferencesTree()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var references = await _unitOfWork.References.GetAll(x => x.IsDeleted != true).AsNoTracking().ToListAsync();

            var referencesTree = GenerateReferenceTree(references, null).Adapt<List<Dtos.ReferencesTree>>(Dtos.ReferencesTree.SelectConfig());


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.ReferencesEntity, referencesTree));
        }

        public async Task<OperationOutput> GetAllMajorsWithReferencesTree(Dtos.References RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var maxId = await _unitOfWork.References.GetAll(x => x.IsDeleted != true).Select(x => x.Id).MaxAsync();

            var majors = await _unitOfWork.ReferencesMajor.GetAll()
                .Where(x => RequestedData.ReferencesMajorId != null? x.Id == RequestedData.ReferencesMajorId : true)
                .Where(x => x.IsDeleted != true)
                .Select(x => new Models.Reference(){   Id = x.Id + maxId, NameAr = x.NameAr, NameEn = x.NameEn,ReferencesMajorId = x.Id})
                .AsNoTracking().ToListAsync();

            var references = await _unitOfWork.References.GetAll()
                .Where(x => RequestedData.ReferencesMajorId != null ? x.ReferencesMajorId == RequestedData.ReferencesMajorId : true)
                .Where(x => x.IsDeleted != true)
                .Select(x => new Models.Reference() { Id = x.Id, NameAr = x.NameAr, NameEn = x.NameEn, ReferencesMajorId = x.ReferencesMajorId, ParentId = x.ParentId == null ? x.ReferencesMajorId + maxId : x.ParentId})
                .AsNoTracking().ToListAsync();

            references.AddRange(majors);

            var referencesTree = GenerateReferenceTree(references, null).Adapt<List<Dtos.ReferencesTree>>(Dtos.ReferencesTree.SelectConfig());


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.ReferencesEntity, referencesTree));
        }


        #region HELPER METHOD >> GenerateTree
        public IEnumerable<Dtos.ReferencesTree> GenerateReferenceTree(List<Models.Reference> references, int? parentId)
        {
            foreach (var r in references.Where(r => r.ParentId == parentId))
            {
                var t = r.Adapt<Dtos.ReferencesTree>(Dtos.ReferencesTree.SelectConfig());
                t.Children = GenerateReferenceTree(references, r.Id);
                yield return t;
            }
        }

        #endregion




    }
}
