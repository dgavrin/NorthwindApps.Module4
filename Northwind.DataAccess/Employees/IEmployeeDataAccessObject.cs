using System.Collections.Generic;

namespace Northwind.DataAccess.Employees
{
    /// <summary>
    /// Represents a DAO for Northwind employees.
    /// </summary>
#pragma warning disable CA1040
    public interface IEmployeeDataAccessObject
#pragma warning restore CA1040
    {
        int InsertEmployee(EmployeeTransferObject employee);

        bool DeleteEmployee(int employeeId);

        bool UpdateEmployee(EmployeeTransferObject employee);

        EmployeeTransferObject FindEmployee(int employeeId);

        IList<EmployeeTransferObject> SelectEmployees(int offset, int limit);
    }
}
