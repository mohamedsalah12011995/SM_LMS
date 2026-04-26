
using RM.FAQs.Dtos;

namespace RM.FAQs.Services
{
    public interface IFAQService
    {
        Task<OperationOutput> GetFAQList(Dtos.FAQ RequestedData);
        Task<OperationOutput> GetFAQDetails(Dtos.FAQ RequestedData);
        Task<OperationOutput> GetFAQDetails(int Id);
        Task<OperationOutput> SaveFAQ(Dtos.FAQ RequestedData);
        Task<OperationOutput> FAQModelActions(Dtos.FAQ RequestedData);
    }
}
