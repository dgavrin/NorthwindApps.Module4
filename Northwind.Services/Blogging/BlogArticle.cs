using System;

namespace Northwind.Services.Blogging
{
    public class BlogArticle
    {
        public int BlogArticleId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime PublicationDate { get; set; }

        public int EmployeeId { get; set; }
    }
}
