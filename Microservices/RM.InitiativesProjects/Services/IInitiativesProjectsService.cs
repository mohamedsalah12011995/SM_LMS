using RM.InitiativesProjects.Dtos;
namespace RM.InitiativesProjects.Services
{
    public interface IInitiativesProjectsService
    {
        Task<OperationOutput> GetLookups();
        Task<OperationOutput> GetInitiativesProjectList(Dtos.InitiativesProjects RequestedData);
        Task<OperationOutput> GetInitiativesProjectDetails(Dtos.InitiativesProjects RequestedData);
        Task<OperationOutput> SaveInitiativesProject(Dtos.InitiativesProjects RequestedData);
        Task<OperationOutput> ModelActions(Dtos.InitiativesProjects RequestedData);
    }
}
