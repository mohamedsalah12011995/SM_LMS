

using RM.Core.Services;
using RM.Users.UnitOfWorks;
using RM.Users.Dtos;
using RM.Core.Extensions;
using RM.Core.Helpers;
using static RM.Users.Dtos.OperationOutput;

namespace RM.Users.Services;


public class EmployeeService : BaseService, IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    string EmployeesPayrollInformation = string.Empty;
    string EmployeesInformation = string.Empty;

    public EmployeeService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor, unitOfWork.Configuration)
    {
        _unitOfWork = unitOfWork;
        SetEmployeeInfoConfiguration();
    }

    #region HELPER METHODS >> CONSTRACTOR
    private void SetEmployeeInfoConfiguration()
    {
        EmployeesPayrollInformation = _unitOfWork.Configuration.ReadConfigurationFromSection("EmployeesPayrollInformation");
        EmployeesInformation = _unitOfWork.Configuration.ReadConfigurationFromSection("EmployeesInformation");
    }

    #endregion


    public async Task<OperationOutput> GetEmployeeInformation(Dtos.Users user)
    {
        EmployeeInformation employee = await InvokeService<EmployeeInformation>.Invoke(EmployeesInformation, user);

        if (employee.Body == null)
            return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

        return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
         new OutputDictionary(OperationOutputKeys.EmployeeEntity, employee));
    }

    public async Task<OperationOutput> GetPayrollInformation(Dtos.Users user)
    {

        EmployeePayroll employee = await InvokeService<EmployeePayroll>.Invoke(EmployeesPayrollInformation, user);
        if (employee.Body == null)
            return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

        return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
       new OutputDictionary(OperationOutputKeys.EmployeeEntity, employee));

    }
}
