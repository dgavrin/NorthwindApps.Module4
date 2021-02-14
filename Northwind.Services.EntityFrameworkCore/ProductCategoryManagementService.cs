using System;
using System.Collections.Generic;
using System.Linq;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFrameworkCore
{
    /// <summary>
    /// Represents a product category management service.
    /// </summary>
    public class ProductCategoryManagementService : IProductCategoryManagementService
    {
        private readonly NorthwindContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryManagementService"/> class.
        /// </summary>
        /// <param name="context">Northwind context.</param>
        public ProductCategoryManagementService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public int CreateCategory(ProductCategory productCategory)
        {
            if (productCategory is null)
            {
                return -1;
            }

            if (this.context.ProductCategories.Any())
            {
                productCategory.Id = this.context.ProductCategories.Max(c => c.Id) + 1;
            }
            else
            {
                productCategory.Id = 0;
            }

            this.context.ProductCategories.Add(productCategory);
            this.context.SaveChanges();
            return productCategory.Id;
        }

        /// <inheritdoc/>
        public bool DestroyCategory(int categoryId)
        {
            var category = this.context.ProductCategories.Find(categoryId);
            if (category is not null)
            {
                this.context.ProductCategories.Remove(category);
                this.context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public IList<ProductCategory> LookupCategoriesByName(IList<string> names)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IList<ProductCategory> ShowCategories(int offset, int limit)
        {
            return this.context.ProductCategories.Where(c => c.Id >= offset).Take(limit).ToList();
        }

        /// <inheritdoc/>
        public bool TryShowCategory(int categoryId, out ProductCategory productCategory)
        {
            productCategory = this.context.ProductCategories.Find(categoryId);
            return productCategory is not null;
        }

        /// <inheritdoc/>
        public bool UpdateCategories(int categoryId, ProductCategory productCategory)
        {
            if (productCategory is null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            var category = this.context.ProductCategories.Single(c => c.Id == categoryId);
            if (category is not null)
            {
                category.Name = productCategory.Name;
                category.Description = productCategory.Description;
                this.context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
