using RM.Permits.Dtos;

namespace RM.Permits.Handlers
{
    public interface IProjectService
    {
        Task<OperationOutput> GetFlowStepperList();
        Task<OperationOutput> GetProjectLookups();
        OperationOutput GetUserProjectLookups();
        Task<OperationOutput> GetProjectList(Dtos.Project RequestedData);
        Task<OperationOutput> SaveProject(Dtos.Project RequestdData);
        Task<OperationOutput> GetProjectDetails(Dtos.ProjectDetailsDto RequestedData);
        Task<OperationOutput> ModelAction(Dtos.Project RequestedData);
        Task<OperationOutput> GetCompanyRepresentativeUsers(UserProject RequestedData);
        Task<OperationOutput> GetEmployees(UserProject RequestedData);
        Task<OperationOutput> SaveProjectUsers(List<Dtos.ProjectUsers> projectUsers);
        Task<OperationOutput> GetProjectUsers(ProjectUsers RequestedData);
    }
}