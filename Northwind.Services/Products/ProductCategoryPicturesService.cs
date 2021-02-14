using System;
using System.IO;
using Northwind.Services.Data;

namespace Northwind.Services.Products
{
    /// <summary>
    /// Represents a product category managment service.
    /// </summary>
    public class ProductCategoryPicturesService : IProductCategoryPicturesService
    {
        private readonly NorthwindContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryPicturesService"/> class.
        /// </summary>
        /// <param name="context">Northwind context.</param>
        public ProductCategoryPicturesService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public bool TryShowPicture(int categoryId, out byte[] bytes)
        {
            if (categoryId < 1)
            {
                throw new ArgumentException("CategoryId can't be less than one.", nameof(categoryId));
            }

            var category = this.context.ProductCategories.Find(categoryId);
            if (category is null)
            {
                bytes = null;
                return false;
            }

            bytes = category.Picture;
            return true;
        }

        /// <inheritdoc/>
        public bool UpdatePicture(int categoryId, Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var category = this.context.ProductCategories.Find(categoryId);
            if (category is null)
            {
                return false;
            }

            using var memoryStream = new MemoryStream();
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(memoryStream);
            category.Picture = memoryStream.ToArray();
            this.context.Update(category);
            this.context.SaveChanges();
            return true;
        }

        /// <inheritdoc/>
        public bool DestroyPicture(int categoryId)
        {
            if (categoryId < 1)
            {
                throw new ArgumentException("CategoryId can't be less than one.", nameof(categoryId));
            }

            var category = this.context.ProductCategories.Find(categoryId);
            if (category is null)
            {
                return false;
            }

            category.Picture = null;
            this.context.Update(category);
            this.context.SaveChanges();
            return true;
        }
    }
}
