using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Competitions.Repositories;
using RM.Models.Competitions;


namespace RM.Competitions.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IConfiguration Configuration { get; }
        IBaseRepository<Attachment> Attachments { get;  }
        IBaseRepository<AttachmentType> AttachmentTypes { get;  }
        IBaseRepository<Competitor> Competitors { get; }
        IBaseRepository<CompetitorsType> CompetitorsTypes { get; }
        IBaseRepository<TeamMember> TeamMembers { get; }
        IDbContextTransaction BeginTransaction();

      
        int Complete();
        Task<int> CompleteAsync();
    }
}