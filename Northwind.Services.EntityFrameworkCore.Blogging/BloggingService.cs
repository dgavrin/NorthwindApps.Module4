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

        /// <inheritdoc/>
        public async Task<int> CreateBlogArticleAsync(BlogArticle blogArticle)
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

        /// <inheritdoc/>
        public async Task<int> CreateLinkToProductForArticleAsync(int blogArticleId, int productId)
        {
            if (blogArticleId <= 0)
            {
                throw new ArgumentNullException(nameof(blogArticleId));
            }

            if (productId <= 0)
            {
                throw new ArgumentNullException(nameof(productId));
            }

            var linkToProductIsExist = (await this.GetProductsForArticleAsync(blogArticleId))
                .Any(p => p.ArticleId == blogArticleId && p.ProductId == productId);
            if (!linkToProductIsExist)
            {
                var blogArticleProduct = new BlogArticleProduct(blogArticleId, productId);
                await this.context.ArticleProduct.AddAsync(blogArticleProduct);
                await this.context.SaveChangesAsync();
                return blogArticleProduct.BlogArticleProductId;
            }
            else
            {
                return -1;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyBlogArticleAsync(int blogArticleId)
        {
            var blogArticle = await this.context.Articles.FindAsync(blogArticleId);
            if (blogArticle is not null)
            {
                this.context.Articles.Remove(blogArticle);
                await this.context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyLinkToProductForArticle(int blogArticleProductId)
        {
            var blogArticleProduct = await this.context.ArticleProduct.FindAsync(blogArticleProductId);
            if (blogArticleProduct is not null)
            {
                this.context.ArticleProduct.Remove(blogArticleProduct);
                await this.context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<IList<BlogArticleProduct>> GetProductsForArticleAsync(int blogArticleId)
        {
            if (blogArticleId <= 0)
            {
                throw new ArgumentNullException(nameof(blogArticleId));
            }

            return this.context.ArticleProduct
                               .Where(p => p.ArticleId == blogArticleId)
                               .ToList();
        }

        /// <inheritdoc/>
        public async Task<IList<BlogArticle>> ShowBlogArticlesAsync(int offset, int limit)
        {
            return this.context.Articles
                               .Where(a => a.BlogArticleId >= offset)
                               .Take(limit)
                               .ToList();
        }

        /// <inheritdoc/>
        public bool TryShowBlogArticle(int blogArticleId, out BlogArticle blogArticle)
        {
            blogArticle = this.context.Articles.Find(blogArticleId);
            return blogArticle is not null;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateBlogArticleAsync(int blogArticleId, BlogArticle blogArticle)
        {
            if (blogArticle is null)
            {
                throw new ArgumentNullException(nameof(blogArticle));
            }

            var article = this.context.Articles.Single(a => a.BlogArticleId == blogArticleId);
            if (article is not null)
            {
                article.Title = blogArticle.Title;
                article.Body = blogArticle.Body;
                article.PublicationDate = blogArticle.PublicationDate;
                await this.context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
