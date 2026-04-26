
using Microsoft.AspNetCore.Mvc;
using RM.WorkFlow.Dtos;
using RM.WorkFlow.Dtos.DyFormEntities;
using RM.WorkFlow.Dtos.IntegrationEntities;
using RM.WorkFlow.Records;
namespace RM.WorkFlow.Services
{
    public interface IDynamicFormWorkflowService
    {
        Task<OperationOutput> GetFormAndStepperActions(Dtos.DyFormEntities.FormView RequestedData);

        Task<OperationOutput> GetReferencesByParentId(Dtos.DyFormEntities.FormView RequestedData);

        Task<OperationOutput> SaveFormValue(FormValueDto RequestedData);
        Task<OperationOutput> SaveAction(Dtos.DyFormEntities.FormValuesActions model);
        Task<OperationOutput> EditActionNoteByUserCreated(Dtos.DyFormEntities.FormValuesActions model);

        Task<OperationOutput> GetFormDataList(DynamicFormWorkflowListDto RequestedData);
        Task<OperationOutput> ModelActions(FormValueDto RequestedData);
        Task<OperationOutput> CheckPersonIntegrationData(IntegrationIInputDto integrationData);

        Task<OperationOutput> GetDetailAndActionStepper(Dtos.DyFormEntities.FormView RequestedData);

        Task<OperationOutput> GetFormUserDataList(DynamicFormWorkflowListDto RequestedData);
        Task<OperationOutput> SendEmail(EngineActionJobRoleEmailDataRecord RequestedData);
        FileStreamResult GetPathOfResource(string fileName);

    }
}
