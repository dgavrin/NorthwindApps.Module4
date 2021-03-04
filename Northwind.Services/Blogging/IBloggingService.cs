using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.Services.Blogging
{
    /// <summary>
    /// Represents a managment service for blog articles.
    /// </summary>
    public interface IBloggingService
    {
        /// <summary>
        /// Creates a new blog article.
        /// </summary>
        /// <param name="blogArticle">A <see cref="BlogArticle"/> to create.</param>
        /// <returns>An identifier of a created blog article.</returns>
        Task<int> CreateBlogArticleAsync(BlogArticle blogArticle);

        /// <summary>
        /// Destroys an exited blog article.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifier.</param>
        /// <returns>True if a blog article is destroyed; otherwise false.</returns>
        Task<bool> DestroyBlogArticleAsync(int blogArticleId);

        /// <summary>
        /// Shows a list of blog articles using specified offset and limit for pagination.
        /// </summary>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="BlogArticle"/>.</returns>
        Task<IList<BlogArticle>> ShowBlogArticlesAsync(int offset, int limit);

        /// <summary>
        /// Try to show a blog article with specified indentifier.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifier.</param>
        /// <param name="blogArticle">A blog article to return.</param>
        /// <returns>Returns true if a blog article is returned; otherwise false.</returns>
        bool TryShowBlogArticle(int blogArticleId, out BlogArticle blogArticle);

        /// <summary>
        /// Updates a blog article.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifier.</param>
        /// <param name="blogArticle">A <see cref="BlogArticle"/></param>
        /// <returns>True if a blog article is updated; otherwise false.</returns>
        Task<bool> UpdateBlogArticleAsync(int blogArticleId, BlogArticle blogArticle);

        /// <summary>
        /// Creates a link to a product for a blog article.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifier.</param>
        /// <param name="productId">A product identifier.</param>
        /// <returns>An identifier of a created link.</returns>
        Task<int> CreateLinkToProductForArticleAsync(int blogArticleId, int productId);

        /// <summary>
        /// Returns all related products for a blog article.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifier.</param>
        /// <returns>All related products.</returns>
        Task<IList<BlogArticleProduct>> GetProductsForArticleAsync(int blogArticleId);

        /// <summary>
        /// Destroys an exited link to a product for a blog article.
        /// </summary>
        /// <param name="blogArticleProductId">A blog article product identifier.</param>
        /// <returns>
        /// True if a link to product for a blog article is destroyed; otherwise false.
        /// </returns>
        Task<bool> DestroyLinkToProductForArticle(int blogArticleProductId);
    }
}
