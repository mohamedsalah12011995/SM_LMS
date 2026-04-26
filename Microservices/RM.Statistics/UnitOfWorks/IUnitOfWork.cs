using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.Statistics.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IMemoryCache MemoryCache { get; }
        IMapper Mappers { get; }
        IConfiguration Configuration { get; }
        IBaseRepository<Rate> Rates { get; }
        IBaseRepository<Comment> Comments { get; }
        IBaseRepository<InteractionStatistic> InteractionStatistic { get; }
        IBaseRepository<GeneralNumbersResult> GeneralNumbersResult { get; }
        IBaseRepository<TotalVisitedByEntityResult> TotalVisitedByEntityResult { get; }
        IBaseRepository<StatisticsResult> StatisticsResult { get; } 
        IBaseRepository<Entity> Entities { get; }
        IBaseRepository<CronSettings> CronSettings { get; }
        IBaseRepository<MajorLookup> MajorLookups { get; }
        IBaseRepository<InteractionStatisticsType> InteractionStatisticsTypes { get; }

        IBaseRepository<Advertisement> Advertisements { get; }
        IBaseRepository<Article> Articles { get; }
        IBaseRepository<Eservice> Eservices { get; }
        IBaseRepository<GovService> GovServices { get; }
        IBaseRepository<News> News { get; }

        IBaseRepository<Models.ScientificLetters> ScientificLetters { get; }
        IBaseRepository<Models.Partner> Partners { get; }
        IBaseRepository<Official> Officials { get; }
        IBaseRepository<Models.Multimedia> Multimedia { get; }
        IBaseRepository<JobAdvertisement> JobAdvertisement { get; }
        IBaseRepository<JobCareer> JobCareers { get; }
        IBaseRepository<Models.FAQ> FAQs { get; }
        IBaseRepository<Document> Documents { get; }
        IBaseRepository<Idea> Ideas { get; }
        IDbContextTransaction BeginTransaction();
        public IEnumerable<EntitiesLatestUpdate> GetFromProcdure(string ProcedureName, string ProcedureParams, params object[] Parameter);

        int Complete();
        Task<int> CompleteAsync();
    }
}