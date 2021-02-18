using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.DataAccess.Employees
{
    /// <summary>
    /// Represents a DAO for Northwind employees.
    /// </summary>
#pragma warning disable CA1040
    public interface IEmployeeDataAccessObject
#pragma warning restore CA1040
    {
        Task<int> InsertEmployeeAsync(EmployeeTransferObject employee);

        Task<bool> DeleteEmployeeAsync(int employeeId);

        Task<bool> UpdateEmployeeAsync(EmployeeTransferObject employee);

        EmployeeTransferObject FindEmployee(int employeeId);

        Task<IList<EmployeeTransferObject>> SelectEmployeesAsync(int offset, int limit);
    }
}
