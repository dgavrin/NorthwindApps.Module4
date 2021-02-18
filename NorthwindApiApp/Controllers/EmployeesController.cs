using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Employees;

namespace NorthwindApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class EmployeesController : Controller
    {
        private IEmployeeManagementService employeeManagementService;

        public EmployeesController(IEmployeeManagementService employeeManagementService)
        {
            this.employeeManagementService = employeeManagementService ?? throw new ArgumentNullException(nameof(employeeManagementService));
        }

        [HttpPost]
        public ActionResult<Employee> CreateEmployee(Employee employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            this.employeeManagementService.CreateEmployee(employee);
            return this.Ok(employee);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetEmployees(int offset = 0, int limit = 10)
        {
            if (offset >= 0 && limit > 0)
            {
                return this.Ok(this.employeeManagementService.ShowEmployees(offset, limit));
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpGet("{employeeId}")]
        public ActionResult<Employee> GetEmployee(int employeeId)
        {
            if (this.employeeManagementService.TryShowEmployee(employeeId, out Employee employee))
            {
                return this.Ok(employee);
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpPut("{employeeId}")]
        public ActionResult UpdateEmployee(int employeeId, Employee employee)
        {
            if (employeeId != employee.Id)
            {
                return this.BadRequest();
            }

            this.employeeManagementService.UpdateEmployee(employeeId, employee);
            return this.NoContent();
        }

        [HttpDelete("{employeeId}")]
        public ActionResult<Employee> DeleteEmployee(int employeeId)
        {
            if (this.employeeManagementService.DestroyEmployee(employeeId))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}
