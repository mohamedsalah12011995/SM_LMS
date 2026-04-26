using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RM.Permits.Dtos;
using RM.Permits.Handlers;
using RM.Permits.Records;
using RM.Permits.Services;

namespace RM.Permits.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PermitsController
    {
        private readonly IPermitService _permitService;
        private readonly IProjectService _projectService;


        public PermitsController(IPermitService permitService, IProjectService projectService)
        {
            _permitService = permitService;
            _projectService = projectService;
        }


        [HttpPost]

        public OperationOutput GetPermitRequestCPLookups()
        => _permitService.GetPermitRequestCPLookups();




        [HttpPost]

        public async Task<OperationOutput> GetPermitRequestsList(GetPermitRequestsListRecord RequestedData)

           => await _permitService.GetPermitRequestsList(RequestedData.Adapt<PermitRequest>());

        [HttpPost]

        public async Task<OperationOutput> GetPermitRequestDetails(PermitRequestGetByID RequestedData)
            => await _permitService.GetPermitRequestDetails(RequestedData);




        [HttpPost]

        public async Task<OperationOutput> ChangeActionPermitRequest(PermitAddAction RequestedData)
            => await _permitService.ChangeActionPermitRequest(RequestedData);


        [HttpPost]

        public async Task<OperationOutput> SavePermitRequest(SavePermitRequestRecord RequestdData)
             => await _permitService.SavePermitRequest(RequestdData.Adapt<PermitRequest>());


        [HttpPost]

        public async Task<OperationOutput> GetPermitRequestLookups(PermitRequestLookup RequestdData)
            => await _permitService.GetPermitRequestLookups(RequestdData);

        [HttpPost]

        public async Task<OperationOutput> GetIntegrationData(UserInformation userInfo)
            => await _permitService.GetIntegrationData(userInfo);


        [HttpPost]

        public async Task<OperationOutput> QueryPersonalPermitRequests(QueryPersonPermitRequests RequestdData)
           => await _permitService.QueryPersonalPermitRequests(RequestdData);


        [HttpPost]

        public async Task<OperationOutput> QueryCarPermitRequests(QueryCarPermitRequests RequestdData)
            => await _permitService.QueryCarPermitRequests(RequestdData);


        [HttpPost]

        public OperationOutput RequestToPrintPermit(PermitRequestGetByID RequestdData)
            => _permitService.RequestToPrintPermit(RequestdData);


        [HttpPost]

        public async Task<OperationOutput> PrintPermit(PrintPermitDto RequestedData)
            => await _permitService.PrintPermit(RequestedData);

        [HttpPost]

        public async Task<OperationOutput> GetCompanyInfo()
            => await _permitService.GetCompanyInfo();


        [HttpPost]

        public async Task<OperationOutput> SaveInteractionStatistics(InteractionStatisticsDto requestData)
            => await _permitService.SaveInteractionStatistics(requestData);



        [HttpPost]

        public Task<OperationOutput> GetFlowStepperList()
            => _projectService.GetFlowStepperList();


        [HttpPost]

        public async Task<OperationOutput> GetProjectList(GetProjectListRecord RequestedData)
            => await _projectService.GetProjectList(RequestedData.Adapt<Dtos.Project>());



        [HttpPost]

        public async Task<OperationOutput> SaveProject(SaveProjectRecord RequestdData)
            => await _projectService.SaveProject(RequestdData.Adapt<Dtos.Project>());


        [HttpPost]

        public async Task<OperationOutput> GetProjectDetails(Dtos.ProjectDetailsDto RequestedData)
            => await _projectService.GetProjectDetails(RequestedData);


        [HttpPost]

        public async Task<OperationOutput> Activation(ActivationRecord RequestedData)
             => await _projectService.ModelAction(RequestedData.Adapt<Dtos.Project>());

        [HttpPost]

        public async Task<OperationOutput> Delete(DeleteRecord RequestedData)
            => await _projectService.ModelAction(RequestedData.Adapt<Dtos.Project>());



        [HttpPost]

        public async Task<OperationOutput> GetProjectLookups()
       => await _projectService.GetProjectLookups();



        [HttpPost]

        public OperationOutput GetUserProjectLookups()
            => _projectService.GetUserProjectLookups();



        [HttpPost]

        public async Task<OperationOutput> GetCompanyRepresentativeUsers(GetCompanyRepresentativeUsersRecord RequestedData)
       => await _projectService.GetCompanyRepresentativeUsers(RequestedData.Adapt<UserProject>());


        [HttpPost]

        public async Task<OperationOutput> GetEmployees(GetEmployeesRecord RequestedData)
       => await _projectService.GetEmployees(RequestedData.Adapt<UserProject>());


        [HttpPost]

        public async Task<OperationOutput> SaveProjectUsers(List<Dtos.ProjectUsers> projectUsers)
            => await _projectService.SaveProjectUsers(projectUsers);


        [HttpPost]

        public async Task<OperationOutput> GetProjectUsers(ProjectUsers RequestedData)
            => await _projectService.GetProjectUsers(RequestedData);

        [AllowAnonymous]
        [HttpGet]
        [Route("{fileName}")]
        public IActionResult Resource(string fileName)
                   => _permitService.GetPathOfResource(fileName);


    }
}
