using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<ActionResult<Employee>> CreateEmployeeAsync(Employee employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            await this.employeeManagementService.CreateEmployeeAsync(employee);
            return this.Ok(employee);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployeesAsync(int offset = 0, int limit = 10)
        {
            if (offset >= 0 && limit > 0)
            {
                return this.Ok(await this.employeeManagementService.ShowEmployeesAsync(offset, limit));
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
        public async Task<ActionResult> UpdateEmployeeAsync(int employeeId, Employee employee)
        {
            if (employeeId != employee.EmployeeId)
            {
                return this.BadRequest();
            }

            await this.employeeManagementService.UpdateEmployeeAsync(employeeId, employee);
            return this.NoContent();
        }

        [HttpDelete("{employeeId}")]
        public async Task<ActionResult<Employee>> DeleteEmployeeAsync(int employeeId)
        {
            if (await this.employeeManagementService.DestroyEmployeeAsync(employeeId))
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
