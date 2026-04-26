using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Permits.Dtos;
using RM.Permits.Handlers;
using RM.Permits.PermitEnum;
using RM.Permits.UnitOfWorks;
using static RM.Permits.Dtos.OperationOutput;

namespace RM.Permits.Services
{
    public class ProjectService : BaseService, IProjectService
    {

        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
           : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }


        #region CP-Manage Projects
        public async Task<OperationOutput> GetFlowStepperList()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var flowStepper = await _unitOfWork.FlowStepper.GetAll().AsNoTracking().ToListAsync();
            var flowStepperDto = flowStepper.Adapt<List<FlowStepper>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.FlowStepperEntity, flowStepperDto));


        }

        public async Task<OperationOutput> GetProjectLookups()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var projects = await _unitOfWork.Projects.GetAll(c => c.IsActive == true && c.IsDeleted == false)
                .AsNoTracking().ToListAsync();
            var projectsDto = projects.Adapt<List<Project>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.ProjectEntity, projects));
        }

        public OperationOutput GetUserProjectLookups()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var projects = _unitOfWork.ProjectsUsers.FindAll(c => c.UserId == RequestOwner.Id.Value, c => c.Project)
                .Select(c => new Project
                {
                    Id = c.Project != null ? c.Project.Id : 0,
                    NameAr = c.Project != null ? c.Project.NameAr : string.Empty,
                    NameEn = c.Project != null ? c.Project.NameEn : string.Empty
                }).ToList();
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.ProjectEntity, projects));


        }

        public async Task<OperationOutput> GetProjectList(Project RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var projects = await _unitOfWork.Projects.FindAllByPagination(RequestedData.Filteration(),
                            RequestedData.Pagination, DefaultPaginationCount, x => x.CreatedDate,
                            OrderBy.Descending, c => c.CreatedByNavigation, c => c.UpdatedByNavigation);

            var projectsDto = projects.Data.Adapt<List<Project>>(Project.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.ProjectEntity, projectsDto),
            new OutputDictionary(OperationOutputKeys.Pagination, projects.Pagination));

        }


        public async Task<OperationOutput> SaveProject(Project RequestdData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.Project projectModel = new();

            if (RequestdData.Id.HasValue)
            {
                projectModel = _unitOfWork.Projects.GetById(RequestdData.Id.Value);

                if (projectModel is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestdData.Adapt(projectModel, RequestdData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.Projects.Update(projectModel);
            }
            else
            {
                RequestdData.Adapt(projectModel, RequestdData.AddConfig(RequestOwner.Id));
                _unitOfWork.Projects.Add(projectModel);
            }

            _unitOfWork.Complete();


            AddFlowProjectSteps(RequestdData, projectModel);

            RequestdData.Id = projectModel.Id;

            return await GetProjectDetails(new ProjectDetailsDto { Id = RequestdData.Id });

        }

        #region HELPER METHOD >> SaveProject
        private void AddFlowProjectSteps(Project RequestdData, Models.Project DbItem)
        {
            if (RequestdData.FlowStepperProject.Any())
            {
                if (RequestdData.Id.HasValue)
                {
                    var projectSteps = _unitOfWork.FlowStepperProjects.FindAll(p => p.ProjectId == RequestdData.Id.Value);
                    if (projectSteps.Any()) { _unitOfWork.FlowStepperProjects.DeleteRange(projectSteps); }
                }
                foreach (var step in RequestdData.FlowStepperProject)
                {
                    var flowStep = new Models.FlowStepperProjects
                    {
                        OrderStep = step.OrderStep,
                        ProjectId = DbItem.Id,
                        StepId = step.StepId,
                    };
                    _unitOfWork.FlowStepperProjects.Add(flowStep);
                }
            }

            _unitOfWork.Complete();
        }

        #endregion

        public async Task<OperationOutput> GetProjectDetails(ProjectDetailsDto RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            int projectsPermits = 0;


            var project = await _unitOfWork.Projects.GetAll()
                 .Include(c => c.CreatedByNavigation)
                 .Include(c => c.UpdatedByNavigation)
                 .AsNoTracking().FirstOrDefaultAsync(c => c.Id == RequestedData.Id.Value);

            var projectDto = project.Adapt<Project>(Project.SelectConfig());

            if (projectDto is not null)

                projectDto.FlowStepperProject = _unitOfWork.FlowStepperProjects
                .FindAll(c => c.ProjectId == RequestedData.Id.Value)
                .OrderBy(c => c.OrderStep).ToList()
                .Adapt<List<FlowStepperProject>>();


            if (projectDto.FlowStepperProject.Count > 0)
            {
                projectsPermits = _unitOfWork.PermitsRequest.FindAll(c => c.ProjectId == projectDto.Id && c.IsActive == true && c.IsDeleted == false
                  && c.PermitState == (int)PermitEnums.PermitRequestStatus.Verified && c.NextStep.Value != projectDto.FlowStepperProject.Last().StepId).Count();
            }
            projectDto.IsDisabeldSteps = projectsPermits > 0 ? true : false;

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
           new OutputDictionary(OperationOutputKeys.ProjectEntity, projectDto));


        }

        public async Task<OperationOutput> ModelAction(Project RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            await _unitOfWork.Projects.ExecuteUpdateAsync(x => RequestedData.IsActive.HasValue,
               sett => sett.SetProperty(x => x.IsActive, RequestedData.IsActive.Value)
               .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
               .SetProperty(y => y.UpdatedDate, TransactionDate));


            await _unitOfWork.Projects.ExecuteUpdateAsync(x => RequestedData.IsDeleted.HasValue,
               sett => sett.SetProperty(x => x.IsDeleted, RequestedData.IsDeleted.Value)
               .SetProperty(d => d.DeletedBy, RequestOwner.Id)
               .SetProperty(y => y.DeletedDate, TransactionDate)
               .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
               .SetProperty(y => y.UpdatedDate, TransactionDate));


            if (RequestedData.IsDeleted.HasValue && RequestedData.IsDeleted.Value == true)

                await _unitOfWork.FlowStepperProjects.ExecuteDeleteAsync(p => p.ProjectId == RequestedData.Id.Value);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

        public async Task<OperationOutput> GetCompanyRepresentativeUsers(UserProject RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            UserProject userProject = new UserProject();


            var project = await _unitOfWork.Projects.FindAsync(c => c.Id == RequestedData.Id);

            if (project is not null)
            {
                userProject.Id = project.Id;
                userProject.NameAr = project.NameAr;
                userProject.NameEn = project.NameEn;

            }

            var users = _unitOfWork.User.FindAll(c => c.IsDeleted == false && c.JobRole == (int)Enums.JobRole.OrganizationEmployee, c => c.Reference)
                .Select(c => new UserLookup
                {
                    Id = c.Id,
                    Name = c.Name,
                    ReferneceNameAr = c.Reference != null ? c.Reference.NameAr : string.Empty,
                    ReferneceNameEn = c.Reference != null ? c.Reference.NameEn : string.Empty,

                }).ToList();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.UsersEntity, users),
            new OutputDictionary(OperationOutputKeys.ProjectEntity, userProject));


        }

        public async Task<OperationOutput> GetEmployees(UserProject RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            UserProject userProject = new UserProject();

            var project = await _unitOfWork.Projects.FindAsync(c => c.Id == RequestedData.Id);
            if (project is not null)
            {
                userProject.Id = project.Id;
                userProject.NameAr = project.NameAr;
                userProject.NameEn = project.NameEn;

            }
            var users = _unitOfWork.User.FindAll(c => c.IsDeleted == false
            && (c.JobRole == (int)Enums.JobRole.PermitSecurityAuditOfficer
            || c.JobRole == (int)Enums.JobRole.PermitProjectManager
            || c.JobRole == (int)Enums.JobRole.PermitPrintingOfficer
            || c.JobRole == (int)Enums.JobRole.PermitApprovalOfficer), c => c.Reference)
            .Select(c => new UserLookup
            {
                Id = c.Id,
                Name = c.Name,
                ReferneceNameAr = c.Reference != null ? c.Reference.NameAr : string.Empty,
                ReferneceNameEn = c.Reference != null ? c.Reference.NameEn : string.Empty,
            }).ToList();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
         new OutputDictionary(OperationOutputKeys.UsersEntity, users),
         new OutputDictionary(OperationOutputKeys.ProjectEntity, userProject));

        }

        public async Task<OperationOutput> SaveProjectUsers(List<ProjectUsers> projectUsers)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<Models.ProjectsUsers> pUsers = new List<Models.ProjectsUsers>();

            if (projectUsers.Any())
            {
                var users = _unitOfWork.ProjectsUsers.FindAll(c => c.ProjectId == projectUsers.First().ProjectId && c.IsEmployee == projectUsers.First().IsEmployee).ToList();
                if (users.Any())
                {
                    _unitOfWork.ProjectsUsers.DeleteRange(users);
                }

                foreach (var item in projectUsers)
                {
                    pUsers.Add(new Models.ProjectsUsers { ProjectId = item.ProjectId.Value, UserId = item.UserId.Value, IsEmployee = item.IsEmployee });
                }
                _unitOfWork.ProjectsUsers.AddRange(pUsers);
                await _unitOfWork.CompleteAsync();

                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

        }

        public async Task<OperationOutput> GetProjectUsers(ProjectUsers RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var projectUsers = new List<UserLookup>();

            if (RequestedData.ProjectId != null)
            {
                projectUsers = await _unitOfWork.ProjectsUsers.GetAll().Where(c => c.ProjectId == RequestedData.ProjectId.Value && c.IsEmployee == RequestedData.IsEmployee)
                    .Select(c => new UserLookup
                    {
                        Id = c.UserId,

                    }).ToListAsync();
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                  new OutputDictionary(OperationOutputKeys.UsersEntity, projectUsers));

        }

        #endregion



    }
}
