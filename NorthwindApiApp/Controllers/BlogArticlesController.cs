using System;
using System.Collections.Generic;
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

            return await this.bloggingService.CreateBlogArticleAsync(blogArticle);
        }

        [HttpDelete("{blogArticleId}")]
        public async Task<ActionResult> DeleteBlogArticleAsync(int blogArticleId)
        {
            if (await this.bloggingService.DestroyBlogArticleAsync(blogArticleId))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogArticleShortResponse>>> GetBlogArticlesAsync([FromServices] IEmployeeManagementService employeeManagementService, int offset = 0, int limit = 10)
        {
            if (offset < 0 ||
                limit <= 0 ||
                employeeManagementService is null)
            {
                return this.BadRequest();
            }

            var blogArticles = await this.bloggingService.ShowBlogArticlesAsync(offset, limit);
            var blogArticleShortResponseList = new List<BlogArticleShortResponse>();
            foreach (var article in blogArticles)
            {
                if (employeeManagementService.TryShowEmployee(article.EmployeeId, out Employee employee))
                {
                    var blogArticleShortResponse = new BlogArticleShortResponse(article, employee);
                    blogArticleShortResponseList.Add(blogArticleShortResponse);
                }
            }

            return this.Ok(blogArticleShortResponseList);
        }

        [HttpGet("{blogArticleId}")]
        public async Task<ActionResult<BlogArticleFullResponse>> GetBlogArticle(int blogArticleId, [FromServices] IEmployeeManagementService employeeManagementService)
        {
            if (blogArticleId <= 0 ||
                employeeManagementService is null)
            {
                return this.BadRequest();
            }

            if (this.bloggingService.TryShowBlogArticle(blogArticleId, out BlogArticle blogArticle))
            {
                employeeManagementService.TryShowEmployee(blogArticle.EmployeeId, out Employee employee);
                return this.Ok(new BlogArticleFullResponse(blogArticle, employee));
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpPut("{blogArticleId}")]
        public async Task<ActionResult> UpdateBlogArticleAsync(int blogArticleId, BlogArticleUpdateQuery blogArticleUpdateQuery)
        {
            if (blogArticleId <= 0)
            {
                return this.BadRequest();
            }

            await this.bloggingService.UpdateBlogArticleAsync(blogArticleId, blogArticleUpdateQuery);
            return this.NoContent();
        }
    }
}
