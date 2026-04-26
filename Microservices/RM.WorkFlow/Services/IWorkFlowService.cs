
using Microsoft.AspNetCore.Mvc;
using RM.WorkFlow.Dtos;

namespace RM.WorkFlow.Services
{
    public interface IWorkFlowService
    {
        Task<OperationOutput> GetWorkflowLookups(Engine RequestedData);
        Task<OperationOutput> GetEnginesList(Engine RequestedData);
        Task<OperationOutput> SaveEngine(Engine RequestedData);
        Task<OperationOutput> GetEngineDetails(Engine RequestedData);
        Task<OperationOutput> EnginesDeleteList(List<Engine> RequestedData);
        Task<OperationOutput> EnginesActivationList(List<Engine> RequestedData);
        Task<OperationOutput> GetAllMajorsWithReferencesTree(ReferencesTreeDto RequestedData);
        Task<OperationOutput> GetWorkFlowActionsList(WorkFlowActions RequestedData);
        Task<OperationOutput> SaveWorkFlowActions(WorkFlowActions RequestedData);
        Task<OperationOutput> GetWorkFlowActionsDetails(WorkFlowActions RequestedData);
        Task<OperationOutput> WorkFlowActionsDeleteList(List<WorkFlowActions> RequestedData);
        Task<OperationOutput> WorkFlowActionsActivationList(List<WorkFlowActions> RequestedData);
        FileStreamResult GetPathOfResource(string fileName);


    }
}
