using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Northwind.DataAccess.Employees;
using Northwind.Services.Employees;

namespace Northwind.DataAccess.SqlServer.Employees
{
    public class EmployeesManagementDataAccessService : IEmployeeManagementService
    {
        private NorthwindDataAccessFactory northwindDataAccessFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesManagementDataAccessService"/> class.
        /// </summary>
        /// <param name="sqlConnection">Sql connection.</param>
        public EmployeesManagementDataAccessService(SqlConnection sqlConnection)
        {
            this.northwindDataAccessFactory = new SqlServerDataAccessFactory(sqlConnection) ?? throw new ArgumentNullException(nameof(sqlConnection));
        }

        /// <inheritdoc/>
        public int CreateEmployee(Employee employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            return this.northwindDataAccessFactory.GetEmployeeDataAccessObject().InsertEmployee((EmployeeTransferObject)employee);
        }

        /// <inheritdoc/>
        public bool DestroyEmployee(int employeeId)
        {
            if (employeeId < 1)
            {
                throw new ArgumentException("EmployeeId can't be less than one.", nameof(employeeId));
            }

            return this.northwindDataAccessFactory.GetEmployeeDataAccessObject().DeleteEmployee(employeeId);
        }

        /// <inheritdoc/>
        public IList<Employee> ShowEmployees(int offset, int limit)
        {
            var employees = new List<Employee>();
            foreach (var employee in this.northwindDataAccessFactory.GetEmployeeDataAccessObject().SelectEmployees(offset, limit))
            {
                employees.Add((Employee)employee);
            }

            return employees;
        }

        /// <inheritdoc/>
        public bool TryShowEmployee(int employeeId, out Employee employee)
        {
            if (employeeId < 1)
            {
                throw new ArgumentException("EmployeeId can't be less than one.", nameof(employeeId));
            }

            try
            {
                employee = (Employee)this.northwindDataAccessFactory.GetEmployeeDataAccessObject().FindEmployee(employeeId);
            }
            catch (Exception)
            {
                employee = null;
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public bool UpdateEmployee(int employeeId, Employee employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            if (employeeId != employee.Id)
            {
                return false;
            }

            if (this.northwindDataAccessFactory.GetEmployeeDataAccessObject().UpdateEmployee((EmployeeTransferObject)employee))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
