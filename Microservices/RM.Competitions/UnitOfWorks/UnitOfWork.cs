using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using RM.Models.Competitions;
using RM.Competitions.Repositories;


namespace RM.Competitions.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private GardensCompetitionContext _context;
        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; } 
        public IBaseRepository<Attachment> Attachments { get; private set; }
        public IBaseRepository<AttachmentType> AttachmentTypes { get; private set; }
        public IBaseRepository<Competitor> Competitors { get; private set; }
        public IBaseRepository<CompetitorsType> CompetitorsTypes { get; private set; }
        public IBaseRepository<TeamMember> TeamMembers { get; private set; }

        public UnitOfWork(GardensCompetitionContext context, ILoggerFactory loggerFactory, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            Attachments = new BaseRepository<RM.Models.Competitions.Attachment>(context);
            AttachmentTypes=new BaseRepository<AttachmentType>(context);
            Competitors = new BaseRepository<Competitor>(context);
            CompetitorsTypes=new BaseRepository<CompetitorsType>(context);
            TeamMembers=new BaseRepository<TeamMember>(context);
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