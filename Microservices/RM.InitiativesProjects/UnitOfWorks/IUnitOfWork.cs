using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;




namespace RM.InitiativesProjects.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IBaseRepository<InitiativesProject> InitiativesProject { get;}
        IBaseRepository<Beneficiary> Beneficiary { get;}
        IBaseRepository<InitiativesProjectsBeneficiary> InitiativesProjectsBeneficiary { get; }
        IBaseRepository<InitiativesProjectsType> InitiativesProjectsType { get; }
        IBaseRepository<Comment> Comments { get; }

        IConfiguration Configuration { get; } 
        IDbContextTransaction BeginTransaction();
      
        int Complete();
        Task<int> CompleteAsync();
    }
}