


using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.GovServices.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IBaseRepository<Models.GovService> GovServices { get; }
        IBaseRepository<Models.Eservice> Eservices { get; }

        IConfiguration Configuration { get; } 
        IDbContextTransaction BeginTransaction();
      
        int Complete();
        Task<int> CompleteAsync();
    }
}