


using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.Feedbacks.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IMemoryCache MemoryCache { get; }
        IBaseRepository<Models.Feedbacks> Feedbacks { get; }
        IBaseRepository<Models.FeedbacksDataSource> FeedbacksDataSources { get; }
        IBaseRepository<Models.FeedbacksAnswerAction> FeedbacksAnswerActions { get; }
        IBaseRepository<Models.FeedbacksAnswer> FeedbacksAnswers { get; }
        IBaseRepository<Recommendations> Recommendations { get; }

        IBaseRepository<Advertisement> Advertisements { get; }
        IBaseRepository<Article> Articles { get; }
        IBaseRepository<Eservice> Eservices { get; }
        IBaseRepository<GovService> GovServices { get; }
        IBaseRepository<News> News { get; }
        IBaseRepository<Entity> Entities { get; }
        IBaseRepository<CronSettings> CronSettings { get; }

        IBaseRepository<Models.ScientificLetters> ScientificLetters { get; }
        IBaseRepository<Models.Partner> Partners { get; }
        IBaseRepository<Official> Officials { get; }
        IBaseRepository<Models.Multimedia> Multimedia { get; }
        IBaseRepository<JobAdvertisement> JobAdvertisement { get; }
        IBaseRepository<JobCareer> JobCareers { get; }
        IBaseRepository<Models.FAQ> FAQs { get; }
        IBaseRepository<Document> Documents { get; }
        IBaseRepository<Idea> Ideas { get; }

        IConfiguration Configuration { get; } 
        IDbContextTransaction BeginTransaction();
      
        int Complete();
        Task<int> CompleteAsync();
    }
}