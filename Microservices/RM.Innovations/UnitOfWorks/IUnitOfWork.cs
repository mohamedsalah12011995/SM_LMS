using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using RM.Core.Interfaces;
using RM.Models;


namespace RM.Innovations.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IMemoryCache MemoryCache { get; }

        IBaseRepository<Idea> Ideas { get; }
        IBaseRepository<IdeaAction> IdeaActions { get; }
        IBaseRepository<IdeaComment> IdeaComments { get; }
        IBaseRepository<IdeasCompetentAuthority> IdeasCompetentAuthority { get; }
        IBaseRepository<User> Users { get; }
        IBaseRepository<Reference> References { get; }
        IBaseRepository<MajorLookup> MajorLookups { get; }
        IBaseRepository<CronSettings> CronSettings { get; }
        IBaseRepository<Entity> Entities { get; }

        IConfiguration Configuration { get; } 
        IDbContextTransaction BeginTransaction();
      
        int Complete();
        Task<int> CompleteAsync();
    }
}