using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;
using NorthwindApiApp.Models;

namespace NorthwindApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class BlogArticlesController : Controller
    {
        private IBloggingService bloggingService;

        public BlogArticlesController(IBloggingService bloggingService)
        {
            this.bloggingService = bloggingService ?? throw new ArgumentNullException(nameof(bloggingService));
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateBlogArticleAsync(BlogArticleDTO blogArticle, [FromServices] IEmployeeManagementService employeeManagementService)
        {
            if (blogArticle is null ||
                employeeManagementService is null ||
                !employeeManagementService.TryShowEmployee(blogArticle.EmployeeId, out _))
            {
                return this.BadRequest();
            }

            return await this.bloggingService.CreateBlogArticle(blogArticle);
        }
    }
}
