using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;

namespace NorthwindApiApp.Models
{
    public class BlogArticleFullResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogArticleFullResponse"/> class.
        /// </summary>
        public BlogArticleFullResponse()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogArticleFullResponse"/> class.
        /// </summary>
        /// <param name="blogArticle">A <see cref="BlogArticle"/>.</param>
        /// <param name="employee">An <see cref="Employee"/>.</param>
        public BlogArticleFullResponse(BlogArticle blogArticle, Employee employee)
        {
            this.Id = blogArticle.BlogArticleId;
            this.Title = blogArticle.Title;
            this.PulicationDate = blogArticle.PublicationDate;
            this.EmployeeId = blogArticle.EmployeeId;
            this.AuthorName = $"{employee.LastName} {employee.FirstName}";
            this.Body = blogArticle.Body;
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        public DateTime PulicationDate { get; set; }

        public int EmployeeId { get; set; }

        public string AuthorName { get; set; }

        [StringLength(4000)]
        public string Body { get; set; }
    }
}
