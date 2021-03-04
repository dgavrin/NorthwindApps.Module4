using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Services.Blogging;
using Northwind.Services.EntityFrameworkCore.Blogging.Context;

namespace Northwind.Services.EntityFrameworkCore.Blogging
{
    public class BloggingService : IBloggingService
    {
        private BloggingContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BloggingService"/> class.
        /// </summary>
        /// <param name="bloggingContext">A <see cref="BloggingContext"/>.</param>
        public BloggingService(BloggingContext bloggingContext)
        {
            this.context = bloggingContext ?? throw new ArgumentNullException(nameof(bloggingContext));
        }

        public async Task<int> CreateBlogArticle(BlogArticle blogArticle)
        {
            if (blogArticle is null)
            {
                throw new ArgumentNullException(nameof(blogArticle));
            }

            blogArticle.BlogArticleId = 0;
            await this.context.Articles.AddAsync(blogArticle);
            await this.context.SaveChangesAsync();
            return blogArticle.BlogArticleId;
        }
    }
}
