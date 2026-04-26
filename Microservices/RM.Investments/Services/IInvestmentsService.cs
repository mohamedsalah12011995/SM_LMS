using RM.Core.Services;
using RM.Investments.Dtos;

namespace RM.Investments.Services
{
    public interface IInvestmentsService:IBaseService
    {
        Task<OperationOutput> GetInvestmentDetails(Dtos.Investments RequestedData);
        Task<OperationOutput> GetInvestmentDetails(int Id);
        Task<OperationOutput> GetInvestmentsTypes();
        Task<OperationOutput> GetInvestmentsList(Dtos.Investments RequestedData);
        Task<OperationOutput> SaveInvestment(Dtos.Investments RequestdData);
        Task<OperationOutput> ModelAction(Dtos.Investments RequestedData);
    }
}
