
using RM.OpenData.Dtos;

namespace RM.OpenData.Services
{
    public interface IOpenDataTempService
    {

        Task<OperationOutput> SyncWithOpenDataTemp(Dtos.RequestRecords RequestedData);
        Task<OperationOutput> GetOpenDataTempList(Dtos.OpenData RequestedData);
        Task<OperationOutput> GetOpenDataTempDetails(Dtos.OpenData RequestedData);
        Task<OperationOutput> GetOpenDataTempByFiledDetails(Dtos.OpenData RequestedData);
        Task<OperationOutput> DeleteOpenDataTemp(List<Dtos.OpenData> RequestedData);

        Task<OperationOutput> ConfirmOpenDataTemp(List<Dtos.OpenData> RequestedData);
        Task<OperationOutput> SaveOpenDataTemp(Dtos.OpenData RequestedData);
        Task<OperationOutput> SyncAllWithOpenData(Dtos.OpenData RequestedData);
        Task<OperationOutput> SyncWithOpenData(List<Dtos.OpenData> RequestedDataList);
        Task<OperationOutput> ConfirmAllOpenDataTemp(Dtos.OpenData RequestedData);


    }
}
