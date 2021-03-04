using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Services.Blogging
{
    public class BlogComment
    {
        public int BlogCommentId { get; set; }

        public int ArticleId { get; set; }

        public string Text { get; set; }

        public int CustomerId { get; set; }
    }
}
