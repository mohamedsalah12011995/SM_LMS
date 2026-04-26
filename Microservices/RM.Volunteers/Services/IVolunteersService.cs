using RM.Core.Services;
using RM.Volunteers.Dtos;
namespace RM.Volunteers.Services
{
    public interface IVolunteersService:IBaseService
    {
        Task<OperationOutput> SaveVolunteers(Dtos.Volunteers RequestdData);
        Task<OperationOutput> GetVolunteersList(Dtos.Volunteers RequestedData);
        Task<OperationOutput> GetVolunteersLookups();
        Task<OperationOutput> GetVolunteersDetails(Dtos.Volunteers RequestedData);
    }
}
