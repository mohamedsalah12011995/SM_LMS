using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using RM.Core.Interfaces;
using RM.Models;


namespace RM.ContactUs.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IMapper Mappers { get; }
        IMemoryCache MemoryCache { get; }

        IBaseRepository<ContactU> ContactUs { get; }
        IBaseRepository<User> User { get; }
        IBaseRepository<Reference> References { get; }
        IBaseRepository<Feedback> Feedback { get; }
        IBaseRepository<Actions> Actions { get; }

        IBaseRepository<Eservice> Eservices { get; }
        IBaseRepository<GovService> GovServices { get; }

        IBaseRepository<Entity> Entities { get; }
        IBaseRepository<CronSettings> CronSettings { get; }
        IBaseRepository<MajorStatus> MajorStatus { get; }
        IBaseRepository<InteractionStatistic> InteractionStatistic { get; }
        IBaseRepository<Recommendations> Recommendations { get; }
        IConfiguration Configuration { get; } 
        IDbContextTransaction BeginTransaction();
      
        int Complete();
        Task<int> CompleteAsync();
    }
}