using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Services.Blogging;

namespace NorthwindApiApp.Models
{
    public class BlogArticleDTO
    {
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(4000)]
        public string Body { get; set; }

        public int EmployeeId { get; set; }

        public static implicit operator BlogArticle(BlogArticleDTO blogArticle)
        {
            return new BlogArticle
            {
                BlogArticleId = 0,
                Title = blogArticle.Title,
                Body = blogArticle.Body,
                PublicationDate = DateTime.Now,
                EmployeeId = blogArticle.EmployeeId,
            };
        }
    }
}
