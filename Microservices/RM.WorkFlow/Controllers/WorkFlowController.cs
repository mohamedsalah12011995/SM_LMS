using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.WorkFlow.Dtos;
using RM.WorkFlow.Dtos.DyFormEntities;
using RM.WorkFlow.Dtos.IntegrationEntities;
using RM.WorkFlow.Records;
using RM.WorkFlow.Services;

namespace RM.WorkFlow.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkFlowController : ControllerBase
    {
        private readonly IWorkFlowService _workFlowService;
        private readonly IDynamicFormWorkflowService _dynamicFormService;

        public WorkFlowController(IWorkFlowService workFlowService, IDynamicFormWorkflowService dynamicFormService)
        {
            _workFlowService = workFlowService;
            _dynamicFormService = dynamicFormService;
        }

        [HttpPost]

        public async Task<OperationOutput> GetWorkflowLookups(GetWorkflowLookupsRecord RequestedData)
             => await _workFlowService.GetWorkflowLookups(RequestedData.Adapt<Engine>());


        [HttpPost]

        public async Task<OperationOutput> GetEnginesList(GetEnginesListRecord RequestedData)
            => await _workFlowService.GetEnginesList(RequestedData.Adapt<Engine>());

        [HttpPost]

        public async Task<OperationOutput> SaveEngine(SaveEngineRecord RequestedData)
             => await _workFlowService.SaveEngine(RequestedData.Adapt<Engine>());

        [HttpPost]

        public async Task<OperationOutput> GetEngineDetails(GetEngineDetailsRecord RequestedData)
             => await _workFlowService.GetEngineDetails(RequestedData.Adapt<Engine>());


        [HttpPost]

        public async Task<OperationOutput> EnginesDeleteList(List<EnginesDeleteRecord> RequestedData)
             => await _workFlowService.EnginesDeleteList(RequestedData.Adapt<List<Engine>>());

        [HttpPost]

        public async Task<OperationOutput> EnginesActivationList(List<EnginesActivationRecord> RequestedData)
            => await _workFlowService.EnginesActivationList(RequestedData.Adapt<List<Engine>>());


        [HttpPost]

        public async Task<OperationOutput> GetWorkFlowActionsList(GetWorkFlowActionsListRecord RequestedData)
            => await _workFlowService.GetWorkFlowActionsList(RequestedData.Adapt<WorkFlowActions>());

        [HttpPost]

        public async Task<OperationOutput> SaveWorkFlowActions(SaveWorkFlowActionsRecord RequestedData)
             => await _workFlowService.SaveWorkFlowActions(RequestedData.Adapt<WorkFlowActions>());



        [HttpPost]

        public async Task<OperationOutput> GetWorkFlowActionsDetails(GetWorkFlowActionsDetailsRecord RequestedData)
           => await _workFlowService.GetWorkFlowActionsDetails(RequestedData.Adapt<WorkFlowActions>());

        [HttpPost]

        public async Task<OperationOutput> WorkFlowActionsDeleteList(List<WorkFlowActionsDeleteRecord> RequestedData)
             => await _workFlowService.WorkFlowActionsDeleteList(RequestedData.Adapt<List<WorkFlowActions>>());

        [HttpPost]

        public async Task<OperationOutput> WorkFlowActionsActivationList(List<WorkFlowActionsActivationRecord> RequestedData)
             => await _workFlowService.WorkFlowActionsActivationList(RequestedData.Adapt<List<WorkFlowActions>>());

        [HttpPost]

        public async Task<OperationOutput> GetAllMajorsWithReferencesTree(GetAllMajorsWithReferencesTreeRecord RequestedData)
           => await _workFlowService.GetAllMajorsWithReferencesTree(RequestedData.Adapt<ReferencesTreeDto>());

        #region Dynamic Form Workflow

        [HttpPost]

        public async Task<OperationOutput> GetFormAndStepperActions(GetFormAndStepperActionsRecord RequestedData)
            => await _dynamicFormService.GetFormAndStepperActions(RequestedData.Adapt<FormView>());


        [HttpPost]

        public async Task<OperationOutput> GetReferencesByParentId(GetReferencesByParentIdRecord RequestedData)
            => await _dynamicFormService.GetReferencesByParentId(RequestedData.Adapt<FormView>());


        [HttpPost]

        public async Task<OperationOutput> SaveFormValue(SaveFormValueRecord RequestedData)
            => await _dynamicFormService.SaveFormValue(RequestedData.Adapt<FormValueDto>());


        [HttpPost]

        public async Task<OperationOutput> SaveAction(SaveActionRecord model)
          => await _dynamicFormService.SaveAction(model.Adapt<Dtos.DyFormEntities.FormValuesActions>());


        [HttpPost]

        public async Task<OperationOutput> EditActionNoteByUserCreated(EditActionNoteByUserCreatedRecord model)
          => await _dynamicFormService.EditActionNoteByUserCreated(model.Adapt<Dtos.DyFormEntities.FormValuesActions>());



        [HttpPost]

        public async Task<OperationOutput> GetFormDataList(GetFormDataListRecord RequestedData)
        => await _dynamicFormService.GetFormDataList(RequestedData.Adapt<DynamicFormWorkflowListDto>());


        [HttpPost]

        public async Task<OperationOutput> ModelActions(ModelActionsRecord RequestedData)
       => await _dynamicFormService.ModelActions(RequestedData.Adapt<FormValueDto>());

        [HttpPost]
        public async Task<OperationOutput> CheckPersonIntegrationData(IntegrationIInputDto integrationData)
            => await _dynamicFormService.CheckPersonIntegrationData(integrationData);


        [HttpPost]
        public async Task<OperationOutput> GetDetailAndActionStepper(Dtos.DyFormEntities.FormView RequestedData)
           => await _dynamicFormService.GetDetailAndActionStepper(RequestedData);

        [HttpPost]
        public async Task<OperationOutput> GetFormUserDataList(DynamicFormWorkflowListDto RequestedData)
             => await _dynamicFormService.GetFormUserDataList(RequestedData);

        [HttpPost]
        public async Task<OperationOutput> SendEmail(EngineActionJobRoleEmailDataRecord RequestedData)
          => await _dynamicFormService.SendEmail(RequestedData);



        [AllowAnonymous]
        [HttpGet]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
             => _workFlowService.GetPathOfResource(fileName);


        #endregion

    }
}
