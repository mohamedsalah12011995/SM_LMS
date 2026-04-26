using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;




namespace RM.Articles.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IBaseRepository<Article> Articles { get; }
        IConfiguration Configuration { get; } 
        IDbContextTransaction BeginTransaction();
      
        int Complete();
        Task<int> CompleteAsync();
    }
}