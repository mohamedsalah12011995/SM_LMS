

using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.DynamicForms.Dtos;
using RM.DynamicForms.Dtos.DynamicForm.FormModuleDtos;
using RM.DynamicForms.Dtos.DynamicForm.IntegrationEntities;
using RM.DynamicForms.Dtos.DynamicForm.TemplateDtos;
using RM.DynamicForms.Records;
using RM.DynamicForms.Services;

namespace Surveys.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DynamicFormsController : ControllerBase
    {
        private readonly IDynamicFormService _dynamicFormService;

        public DynamicFormsController(IDynamicFormService DynamicFormService)
              => _dynamicFormService = DynamicFormService;

        [HttpPost]

        public async Task<OperationOutput> GetFormsList(GetFormsListRecord RequestedData)
            => await _dynamicFormService.GetFormsList(RequestedData.Adapt<Form>());


        [HttpPost]

        public async Task<OperationOutput> GetFormLookups()
            => await _dynamicFormService.GetFormLookups();


        [HttpPost]

        public async Task<OperationOutput> SaveForm(Form RequestedData)
            => await _dynamicFormService.SaveForm(RequestedData);


        [HttpPost]

        public async Task<OperationOutput> GetFormDetails(GetFormDetailsRecord RequestedData)
              => await _dynamicFormService.GetFormDetails(RequestedData.Adapt<Form>());

        [HttpPost]

        public async Task<OperationOutput> FormActivation(FormActions RequestedData)
            => await _dynamicFormService.FormActivation(RequestedData);

        [HttpPost]

        public async Task<OperationOutput> FormDeletion(FormActions RequestedData)
           => await _dynamicFormService.FormDeletion(RequestedData);


        [HttpPost]

        public async Task<OperationOutput> GetFormForView(GetFormForViewRecord RequestedData)
           => await _dynamicFormService.GetFormForView(RequestedData.Adapt<Form>());


        [HttpPost]

        public async Task<OperationOutput> SaveFormValue(SaveFormValueRecord RequestedData)
            => await _dynamicFormService.SaveFormValue(RequestedData.Adapt<FormValue>());



        [HttpPost]

        public async Task<OperationOutput> GetFormDataList(DynamicFormListDto RequestedData)
            => await _dynamicFormService.GetFormDataList(RequestedData);


        [HttpPost]

        public async Task<OperationOutput> Activation(ActivationFormValueRecord RequestedData)
             => await _dynamicFormService.ModelActions(RequestedData.Adapt<ListFormValue>());


        [HttpPost]

        public async Task<OperationOutput> Delete(DeleteFormValueRecord RequestedData)
            => await _dynamicFormService.ModelActions(RequestedData.Adapt<ListFormValue>());


        [HttpPost]

        public async Task<OperationOutput> GetFormValueDetail(GetFormValueDetailRecord RequestedData)
            => await _dynamicFormService.GetFormValueDetail(RequestedData.Adapt<ListFormValue>());


        [HttpPost]

        public async Task<OperationOutput> GetDataList(DynamicFormListDto RequestedData)
            => await _dynamicFormService.GetDataList(RequestedData);

        [HttpPost]

        public async Task<OperationOutput> GetDetail(GetDetailRecord RequestedData)
            => await _dynamicFormService.GetDetail(RequestedData.Adapt<FormValue>());

        [HttpPost]

        public async Task<OperationOutput> GetKeysToAdvancedSearch(GetKeysToAdvancedSearchRecord RequestedData)
           => await _dynamicFormService.GetKeysToAdvancedSearch(RequestedData.Adapt<Form>());

        [HttpPost]

        public async Task<OperationOutput> AdvancedFilteration(AdvancedSearchDto RequestedData)
             => await _dynamicFormService.AdvancedFilteration(RequestedData);


        [HttpPost]

        public async Task<OperationOutput> GetDepartmentsWithWorkFlow()
             => await _dynamicFormService.GetDepartmentsWithWorkFlow();

        [HttpPost]

        public async Task<OperationOutput> GetAllMajorsWithReferencesTree()
          => await _dynamicFormService.GetAllMajorsWithReferencesTree();

        [HttpPost]

        public async Task<OperationOutput> GetAllEntities()
           => await _dynamicFormService.GetAllEntities();

        [HttpPost]
        public async Task<OperationOutput> SaveAPIDataSource(APIDataSource RequestedData)
              => await _dynamicFormService.SaveAPIDataSource(RequestedData);

        [HttpPost]
        public async Task<OperationOutput> CheckPersonIntegrationData(IntegrationIInputDto integrationData)
            => await _dynamicFormService.CheckPersonIntegrationData(integrationData);


        [HttpPost]
        public async Task<OperationOutput> GetPortalReferences()
            => await _dynamicFormService.GetPortalReferences();

        [HttpPost]
        public async Task<OperationOutput> GetFormsForCloneToReference(GetFormsForCloneToReferenceRecord RequestedData)
            => await _dynamicFormService.GetFormsForCloneToReference(RequestedData.Adapt<Form>());

        [HttpPost]
        public async Task<OperationOutput> CloneFormsByRefernceId(FormCloneDto RequestedData)
            => await _dynamicFormService.CloneFormsByRefernceId(RequestedData);

        [HttpPost]
        public async Task<OperationOutput> GetFormDataForExport(ExportDataDto RequestedData)
            => await _dynamicFormService.GetFormDataForExport(RequestedData);

        [HttpPost]
        public async Task<OperationOutput> AddFormValueViewStatistic(AddFormValueViewStatistic RequestedData)
           => await _dynamicFormService.AddFormValueViewStatistic(RequestedData);


        [AllowAnonymous]
        [HttpGet]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
             => _dynamicFormService.GetPathOfResource(fileName);

    }
}
