using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Services.Blogging;
using Northwind.Services.Employees;

namespace NorthwindApiApp.Models
{
    public class BlogArticleShortResponse
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        public DateTime PulicationDate { get; set; }

        public int EmployeeId { get; set; }

        public string AuthorName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogArticleShortResponse"/> class.
        /// </summary>
        public BlogArticleShortResponse()
        {
        }

        public BlogArticleShortResponse(BlogArticle blogArticle, Employee employee)
        {
            this.Id = blogArticle.BlogArticleId;
            this.Title = blogArticle.Title;
            this.PulicationDate = blogArticle.PublicationDate;
            this.EmployeeId = blogArticle.EmployeeId;
            this.AuthorName = $"{employee.LastName} {employee.FirstName}";
        }
    }
}
