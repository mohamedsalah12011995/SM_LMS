
using Microsoft.AspNetCore.Mvc;
using RM.DynamicForms.Dtos;
using RM.DynamicForms.Dtos.DynamicForm.FormModuleDtos;
using RM.DynamicForms.Dtos.DynamicForm.IntegrationEntities;
using RM.DynamicForms.Dtos.DynamicForm.TemplateDtos;
using RM.DynamicForms.Records;

namespace RM.DynamicForms.Services

{
    public interface IDynamicFormService
    {
        Task<OperationOutput> GetFormLookups();
        Task<OperationOutput> GetDepartmentsWithWorkFlow();
        Task<OperationOutput> GetFormsList(Dtos.DynamicForm.TemplateDtos.Form RequestedData);
        Task<OperationOutput> SaveForm(Dtos.DynamicForm.TemplateDtos.Form RequestedData);
        Task<OperationOutput> GetFormDetails(Dtos.DynamicForm.TemplateDtos.Form RequestedData);
        Task<OperationOutput> FormActivation(Dtos.DynamicForm.TemplateDtos.FormActions RequestedData);
        Task<OperationOutput> FormDeletion(Dtos.DynamicForm.TemplateDtos.FormActions RequestedData);
        Task<OperationOutput> GetFormForView(Dtos.DynamicForm.TemplateDtos.Form RequestedData);
        Task<OperationOutput> SaveFormValue(Dtos.DynamicForm.TemplateDtos.FormValue RequestedData);
        Task<OperationOutput> GetFormDataList(DynamicFormListDto RequestedData);
        Task<OperationOutput> ModelActions(ListFormValue RequestedData);
        Task<OperationOutput> GetFormValueDetail(ListFormValue RequestedData);
        Task<OperationOutput> GetDataList(DynamicFormListDto RequestedData);
        Task<OperationOutput> GetDetail(Dtos.DynamicForm.TemplateDtos.FormValue RequestedData);
        Task<OperationOutput> GetKeysToAdvancedSearch(Dtos.DynamicForm.TemplateDtos.Form RequestedData);
        Task<OperationOutput> AdvancedFilteration(AdvancedSearchDto RequestedData);
        Task<OperationOutput> GetAllMajorsWithReferencesTree();
        Task<OperationOutput> GetAllEntities();
        Task<OperationOutput> SaveAPIDataSource(Dtos.DynamicForm.TemplateDtos.APIDataSource RequestedData);
        Task<OperationOutput> CheckPersonIntegrationData(IntegrationIInputDto integrationData);

        Task<OperationOutput> GetPortalReferences();
        Task<OperationOutput> GetFormsForCloneToReference(Dtos.DynamicForm.TemplateDtos.Form RequestedData);
        Task<OperationOutput> CloneFormsByRefernceId(FormCloneDto RequestedData);
        Task<OperationOutput> GetFormDataForExport(ExportDataDto RequestedData);
        Task<OperationOutput> AddFormValueViewStatistic(AddFormValueViewStatistic RequestedData);
        FileStreamResult GetPathOfResource(string fileName);
    }
}
