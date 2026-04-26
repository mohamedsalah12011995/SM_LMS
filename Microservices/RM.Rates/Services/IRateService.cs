
using RM.Rates.Dtos;

namespace RM.Rates.Services
{
    public interface IRateService
    {
        Task<OperationOutput> InsertRates(Dtos.Rates RequestedData);
    }
}