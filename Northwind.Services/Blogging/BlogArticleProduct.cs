namespace Northwind.Services.Blogging
{
    public class BlogArticleProduct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogArticleProduct"/> class.
        /// </summary>
        public BlogArticleProduct()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogArticleProduct"/> class.
        /// </summary>
        /// <param name="blogArticleId">A blog article identifier.</param>
        /// <param name="productId">A product identifier.</param>
        public BlogArticleProduct(int blogArticleId, int productId)
        {
            this.BlogArticleProductId = 0;
            this.ArticleId = blogArticleId;
            this.ProductId = productId;
        }

        public int BlogArticleProductId { get; set; }

        public int ArticleId { get; set; }

        public int ProductId { get; set; }
    }
}
