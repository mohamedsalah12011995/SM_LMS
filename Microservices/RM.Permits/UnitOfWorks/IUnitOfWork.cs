using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.Permits.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IConfiguration Configuration { get; }

        IBaseRepository<PermitsRequest> PermitsRequest { get; }
        IBaseRepository<PermitAction> PermitActions { get; }
        IBaseRepository<PermitWorkSite> PermitWorkSites { get; }

        IBaseRepository<Project> Projects { get; }
        IBaseRepository<ProjectsUsers> ProjectsUsers { get; }
        IBaseRepository<FlowStepper> FlowStepper { get; }
        IBaseRepository<FlowStepperProjects> FlowStepperProjects { get; }
        IBaseRepository<MajorLookupsType> MajorLookupsType { get; }
        IBaseRepository<MajorLookup> MajorLookup { get; }
        IBaseRepository<Reference> References { get; }

        IBaseRepository<User> User { get; }


        IDbContextTransaction BeginTransaction();



        int Complete();
        Task<int> CompleteAsync();
    }
}