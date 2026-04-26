using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;

namespace RM.Permits.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public IBaseRepository<PermitsRequest> PermitsRequest { get; private set; }
        public IBaseRepository<PermitAction> PermitActions { get; private set; }
        public IBaseRepository<PermitWorkSite> PermitWorkSites { get; private set; }
        public IBaseRepository<Project> Projects { get; private set; }
        public IBaseRepository<ProjectsUsers> ProjectsUsers { get; private set; }
        public IBaseRepository<FlowStepper> FlowStepper { get; private set; }
        public IBaseRepository<FlowStepperProjects> FlowStepperProjects { get; private set; }
        public IBaseRepository<MajorLookupsType> MajorLookupsType { get; private set; }
        public IBaseRepository<MajorLookup> MajorLookup { get; private set; }

        public IBaseRepository<Reference> References { get; private set; }

        public IBaseRepository<User> User { get; private set; }

        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            PermitsRequest = new BaseRepository<PermitsRequest>(context);
            PermitActions = new BaseRepository<PermitAction>(context);
            PermitWorkSites = new BaseRepository<PermitWorkSite>(context);
            Projects = new BaseRepository<Project>(context);
            ProjectsUsers = new BaseRepository<ProjectsUsers>(context);
            FlowStepper = new BaseRepository<FlowStepper>(context);
            FlowStepperProjects = new BaseRepository<FlowStepperProjects>(context);
            MajorLookupsType = new BaseRepository<MajorLookupsType>(context);
            MajorLookup = new BaseRepository<MajorLookup>(context);
            References = new BaseRepository<Reference>(context);
            User = new BaseRepository<User>(context);
        }


        public void Dispose()
        {
            _context.Dispose();
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }
    }
}