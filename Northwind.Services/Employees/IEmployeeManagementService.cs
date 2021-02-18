using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Services.Employees
{
    public interface IEmployeeManagementService
    {
        Task<IList<Employee>> ShowEmployeesAsync(int offset, int limit);

        bool TryShowEmployee(int employeeId, out Employee employee);

        Task<int> CreateEmployeeAsync(Employee employee);

        Task<bool> DestroyEmployeeAsync(int employeeId);

        Task<bool> UpdateEmployeeAsync(int employeeId, Employee employee);
    }
}
