using Microsoft.AspNetCore.Mvc;
using RM.Core.Helpers;
using RM.References.Dtos;

namespace RM.References.Services
{
    public interface IReferenceService
    {
        public Task<OperationOutput> GetLookups(Dtos.References RequestedData);
        public OperationOutput GetMuniicipalitiesType();
        public Task<OperationOutput> GetMainMenu(Dtos.References RequestedData);
        public Task<OperationOutput> GetMenu(Menu RequestedData);
        public OperationOutput DecryptString(string Value);

        public OperationOutput EncryptString(string Value);
        public Task<OperationOutput> GetReferencesList(Enums.ReferenceMajor ReferenceType, Dtos.References RequestedData);
        public Task<OperationOutput> GetReferencesDetails(Dtos.References RequestedData);
        public Task<OperationOutput> GetReferenceContent(Dtos.References RequestedData);
        public Task<OperationOutput> SaveReferenceDetails(ReferenceContent RequestedData);
        public OperationOutput SearchAsText(SearchEngine.Request RequestedData);
        public Task<OperationOutput> GetSearchableEntities();
        public Task<OperationOutput> GetReferenceMajorList(ReferencesMajor RequestedData);
        public Task<OperationOutput> GetMajorReferenceLookup();
        public Task<OperationOutput> GetReferenceMajorDetails(ReferencesMajor RequestedData);
        public Task<OperationOutput> SaveReferenceMajor(ReferencesMajor RequestedData);
        public Task<OperationOutput> ModelActionsReferenceMajor(ReferencesMajor RequestedData);
        public Task<OperationOutput> GetReferencesByFiltrations(Dtos.References RequestedData);

        public Task<OperationOutput> GetReference(Dtos.References RequestedData);
        public Task<OperationOutput> SaveReference(Dtos.References RequestedData);
        public Task<OperationOutput> ModelActionsReferences(Dtos.References RequestedData);
        public Task<OperationOutput> GetReferencesTreeByMajorId(Dtos.References RequestedData);
        public Task<OperationOutput> GetReferencesTree();
        public Task<OperationOutput> GetAllMajorsWithReferencesTree(Dtos.References RequestedData);
        public FileStreamResult GetPathOfResource(string fileName);

    }
}
