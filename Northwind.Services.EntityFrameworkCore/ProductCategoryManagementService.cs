using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<int> CreateCategoryAsync(ProductCategory productCategory)
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
            await this.context.SaveChangesAsync();
            return productCategory.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyCategoryAsync(int categoryId)
        {
            var category = await this.context.ProductCategories.FindAsync(categoryId);
            if (category is not null)
            {
                this.context.ProductCategories.Remove(category);
                await this.context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<IList<ProductCategory>> LookupCategoriesByNameAsync(IList<string> names)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<IList<ProductCategory>> ShowCategoriesAsync(int offset, int limit)
        {
            return this.context.ProductCategories.Where(c => c.Id >= offset).Take(limit).ToList();
        }

        /// <inheritdoc/>
        public bool TryShowCategoryAsync(int categoryId, out ProductCategory productCategory)
        {
            productCategory = this.context.ProductCategories.Find(categoryId);
            return productCategory is not null;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateCategoriesAsync(int categoryId, ProductCategory productCategory)
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
