using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.Core.Helpers;
using RM.References.Dtos;
using RM.References.Records;
using RM.References.Services;


namespace RM.References.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReferencesController : ControllerBase
    {
        private readonly IReferenceService _referenceService;

        public ReferencesController(IReferenceService referenceService)
            => _referenceService = referenceService;

        [HttpPost]

        public async Task<OperationOutput> GetMainMenu(GetMainMenuRecord RequestedData)
            => await _referenceService.GetMainMenu(RequestedData.Adapt<Dtos.References>());


        [HttpPost]

        public async Task<OperationOutput> GetMenu(GetMenuRecord RequestedData)
            => await _referenceService.GetMenu(RequestedData.Adapt<Menu>());


        [HttpPost]

        public OperationOutput GetMuniicipalitiesType()
          => _referenceService.GetMuniicipalitiesType();


        [HttpPost]

        public async Task<OperationOutput> GetMunicipalitiesList(GetMunicipalitiesListRecord RequestedData)
            => await _referenceService.GetReferencesList(Enums.ReferenceMajor.SubMunicipalities, RequestedData.Adapt<Dtos.References>());

        [HttpPost]

        public async Task<OperationOutput> GetSubMunicipalitiesList(GetSubMunicipalitiesListRecord RequestedData)
            => await _referenceService.GetReferencesList(Enums.ReferenceMajor.SubMunicipalities, RequestedData.Adapt<Dtos.References>());

        [HttpPost]

        public async Task<OperationOutput> GetDepartmentsList(GetDepartmentsListRecord RequestedData)
            => await _referenceService.GetReferencesList(Enums.ReferenceMajor.Departments, RequestedData.Adapt<Dtos.References>());


        [HttpPost]

        public async Task<OperationOutput> GetAgenciesList(GetAgenciesListRecord RequestedData)
           => await _referenceService.GetReferencesList(Enums.ReferenceMajor.Agencies, RequestedData.Adapt<Dtos.References>());

        [HttpPost]
        public async Task<OperationOutput> GetReferenceDetails(GetReferenceDetailsRecord RequestedData)
            => await _referenceService.GetReferencesDetails(RequestedData.Adapt<Dtos.References>());

        [HttpPost]

        public async Task<OperationOutput> SaveReferenceDetails(ReferenceContent RequestedData)
           => await _referenceService.SaveReferenceDetails(RequestedData);


        [HttpPost]

        public OperationOutput DecryptString(String Value)
        {
            return _referenceService.DecryptString(Value);
        }

        [HttpPost, HttpGet]

        public OperationOutput EncryptString(String Value)
        {
            return _referenceService.EncryptString(Value);
        }

        [HttpGet, HttpPost]

        public OperationOutput TestService(Dtos.References RequestedData)
        {
            return new OperationOutput()
            {
                Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess),
                Output = new Dictionary<string, object>() { { "EntityID", "Weclome" } }
            };
        }

        [HttpPost]

        public OperationOutput SearchAsText(SearchEngine.Request RequestedData)
            => _referenceService.SearchAsText(RequestedData);


        [HttpPost]

        public async Task<OperationOutput> GetSearchableEntities()
            => await _referenceService.GetSearchableEntities();

        [HttpPost]

        public async Task<OperationOutput> GetReferenceContent(GetReferenceContentRecord RequestedData)
            => await _referenceService.GetReferenceContent(RequestedData.Adapt<Dtos.References>());


        [HttpPost]
        public async Task<OperationOutput> GetReferenceMajorList(GetReferenceMajorListRecord RequestedData)
             => await _referenceService.GetReferenceMajorList(RequestedData.Adapt<Dtos.ReferencesMajor>());

        [HttpPost]
        public async Task<OperationOutput> GetReferenceMajorDetails(GetReferenceMajorDetailsRecord RequestedData)
            => await _referenceService.GetReferenceMajorDetails(RequestedData.Adapt<Dtos.ReferencesMajor>());


        [HttpPost]

        public async Task<OperationOutput> SaveReferenceMajor(SaveReferenceMajorRecord RequestedData)
            => await _referenceService.SaveReferenceMajor(RequestedData.Adapt<Dtos.ReferencesMajor>());


        [HttpPost]

        public async Task<OperationOutput> DeleteReferenceMajor(DeleteReferenceMajorRecord RequestedData)
            => await _referenceService.ModelActionsReferenceMajor(RequestedData.Adapt<Dtos.ReferencesMajor>());


        [HttpPost]
        public async Task<OperationOutput> GetReferencesByFiltrations(GetReferencesByFiltrationsRecord RequestedData)
             => await _referenceService.GetReferencesByFiltrations(RequestedData.Adapt<Dtos.References>());



        [HttpPost]
        public async Task<OperationOutput> GetReference(GetReferenceRecord RequestedData)
              => await _referenceService.GetReference(RequestedData.Adapt<Dtos.References>());


        [HttpPost]
        public async Task<OperationOutput> SaveReference(SaveReferenceRecord RequestedData)
             => await _referenceService.SaveReference(RequestedData.Adapt<Dtos.References>());



        [HttpPost]
        public async Task<OperationOutput> DeleteReference(DeleteReferenceRecord RequestedData)
            => await _referenceService.ModelActionsReferences(RequestedData.Adapt<Dtos.References>());


        [HttpPost]
        public async Task<OperationOutput> GetMajorReferenceLookup()
            => await _referenceService.GetMajorReferenceLookup();


        [HttpPost]
        public async Task<OperationOutput> GetReferencesTree()
            => await _referenceService.GetReferencesTree();


        [HttpPost]
        public async Task<OperationOutput> GetAllMajorsWithReferencesTree(GetAllMajorsWithReferencesTreeRecord RequestedData)
            => await _referenceService.GetAllMajorsWithReferencesTree(RequestedData.Adapt<Dtos.References>());


        [HttpPost]
        public async Task<OperationOutput> GetReferencesTreeByMajorId(GetReferencesTreeByMajorIdRecord RequestedData)
              => await _referenceService.GetReferencesTreeByMajorId(RequestedData.Adapt<Dtos.References>());


        [AllowAnonymous]
        [HttpGet]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
             => _referenceService.GetPathOfResource(fileName);



    }

}
