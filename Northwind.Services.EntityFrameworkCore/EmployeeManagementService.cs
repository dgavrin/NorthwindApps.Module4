using System;
using System.Collections.Generic;
using System.Linq;
using Northwind.Services.Employees;

namespace Northwind.Services.EntityFrameworkCore
{
    public class EmployeeManagementService : IEmployeeManagementService
    {
        private readonly NorthwindContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeManagementService"/> class.
        /// </summary>
        /// <param name="context">Northwind context.</param>
        public EmployeeManagementService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public int CreateEmployee(Employee employee)
        {
            if (employee is null)
            {
                return -1;
            }

            if (this.context.Employees.Any())
            {
                employee.Id = this.context.Employees.Max(e => e.Id) + 1;
            }
            else
            {
                employee.Id = 0;
            }

            this.context.Employees.Add(employee);
            this.context.SaveChanges();
            return employee.Id;
        }

        /// <inheritdoc/>
        public bool DestroyEmployee(int employeeId)
        {
            var employee = this.context.Employees.Find(employeeId);
            if (employee is not null)
            {
                this.context.Employees.Remove(employee);
                this.context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public IList<Employee> ShowEmployees(int offset, int limit)
        {
            return this.context.Employees.Where(e => e.Id >= offset).Take(limit).ToList();
        }

        /// <inheritdoc/>
        public bool TryShowEmployee(int employeeId, out Employee employee)
        {
            employee = this.context.Employees.Find(employeeId);
            return employee is not null;
        }

        /// <inheritdoc/>
        public bool UpdateEmployee(int employeeId, Employee employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            var newEmployee = this.context.Employees.Single(e => e.Id == employeeId);
            if (newEmployee is not null)
            {
                newEmployee.LastName = employee.LastName;
                newEmployee.FirstName = employee.FirstName;
                newEmployee.Title = employee.Title;
                newEmployee.TitleOfCountesy = employee.TitleOfCountesy;
                newEmployee.BirthDate = employee.BirthDate;
                newEmployee.HireDate = employee.HireDate;
                newEmployee.Address = employee.Address;
                newEmployee.City = employee.City;
                newEmployee.Region = employee.Region;
                newEmployee.PostalCode = employee.PostalCode;
                newEmployee.Country = employee.Country;
                newEmployee.HomePhone = employee.HomePhone;
                newEmployee.Extension = employee.Extension;
                newEmployee.Photo = employee.Photo;
                newEmployee.Notes = employee.Notes;
                newEmployee.ReportsTo = employee.ReportsTo;
                newEmployee.PhotoPath = employee.PhotoPath;
                newEmployee.Salary = employee.Salary;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
