using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
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
        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            return await this.northwindDataAccessFactory.GetEmployeeDataAccessObject().InsertEmployeeAsync((EmployeeTransferObject)employee);
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyEmployeeAsync(int employeeId)
        {
            if (employeeId < 1)
            {
                throw new ArgumentException("EmployeeId can't be less than one.", nameof(employeeId));
            }

            return await this.northwindDataAccessFactory.GetEmployeeDataAccessObject().DeleteEmployeeAsync(employeeId);
        }

        /// <inheritdoc/>
        public async Task<IList<Employee>> ShowEmployeesAsync(int offset, int limit)
        {
            var employees = new List<Employee>();
            foreach (var employee in await this.northwindDataAccessFactory.GetEmployeeDataAccessObject().SelectEmployeesAsync(offset, limit))
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
        public async Task<bool> UpdateEmployeeAsync(int employeeId, Employee employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            if (employeeId != employee.EmployeeId)
            {
                return false;
            }

            if (await this.northwindDataAccessFactory.GetEmployeeDataAccessObject().UpdateEmployeeAsync((EmployeeTransferObject)employee))
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
