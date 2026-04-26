
using RM.Orders.Dtos;

namespace RM.Orders.Services
{
    public interface IOrderService
    {
        Task<OperationOutput> GetOrderLookups(Dtos.Order RequestedData);
        Task<OperationOutput> AddOrder(Dtos.Order RequestedData);
        Task<OperationOutput> GetOrderList(Dtos.Order RequestedData);
        Task<OperationOutput> GetOrderDetails(Dtos.Order RequestedData);
        Task<OperationOutput> GetOrderDetails(int? Id);
        Task<OperationOutput> AddOrderAction(Dtos.OrderActions RequestedData);
        Task<OperationOutput> DeleteOrder(Dtos.Order RequestedData);
        Task<OperationOutput> GetOrder(Dtos.Order RequestedData);

    }
}
