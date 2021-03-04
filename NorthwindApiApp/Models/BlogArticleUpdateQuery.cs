using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Services.Blogging;

namespace NorthwindApiApp.Models
{
    public class BlogArticleUpdateQuery
    {

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(4000)]
        public string Body { get; set; }

        public static implicit operator BlogArticle(BlogArticleUpdateQuery blogArticleUpdateQuery)
        {
            return new BlogArticle
            {
                Title = blogArticleUpdateQuery.Title,
                Body = blogArticleUpdateQuery.Body,
                PublicationDate = DateTime.Now,
            };
        }
    }
}
