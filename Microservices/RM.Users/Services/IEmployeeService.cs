using RM.Users.Dtos;
namespace RM.Users.Services
{
    public interface IEmployeeService
    {
        public Task<OperationOutput> GetEmployeeInformation(Dtos.Users user);
        public Task<OperationOutput> GetPayrollInformation(Dtos.Users user);
    }
}
