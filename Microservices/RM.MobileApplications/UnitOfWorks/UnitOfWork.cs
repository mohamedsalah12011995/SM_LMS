using Microsoft.EntityFrameworkCore.Storage;
using MapsterMapper;
using RM.Core.Interfaces;
using RM.Models;
using RM.Core.Repositories;



namespace RM.MobileApplications.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; } 
        public IBaseRepository<Models.MobileApplication> MobileApplication { get; private set; }
        public IBaseRepository<Models.QuestionsAnswer> QuestionsAnswer { get; private set; }
        public IBaseRepository<Models.Attachment> Attachment { get; private set; }
        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            MobileApplication = new BaseRepository<Models.MobileApplication>(context);
            QuestionsAnswer = new BaseRepository<Models.QuestionsAnswer>(context);
            Attachment = new BaseRepository<Models.Attachment>(context);
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