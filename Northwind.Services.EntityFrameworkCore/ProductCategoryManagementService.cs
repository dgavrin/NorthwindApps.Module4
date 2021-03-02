using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Services.EntityFrameworkCore.Context;
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
        public async Task<int> CreateCategoryAsync(Category productCategory)
        {
            if (productCategory is null)
            {
                return -1;
            }

            if (this.context.Categories.Any())
            {
                productCategory.CategoryId = this.context.Categories.Max(c => c.CategoryId) + 1;
            }
            else
            {
                productCategory.CategoryId = 0;
            }

            this.context.Categories.Add(productCategory);
            await this.context.SaveChangesAsync();
            return productCategory.CategoryId;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyCategoryAsync(int categoryId)
        {
            var category = await this.context.Categories.FindAsync(categoryId);
            if (category is not null)
            {
                this.context.Categories.Remove(category);
                await this.context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<IList<Category>> LookupCategoriesByNameAsync(IList<string> names)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<IList<Category>> ShowCategoriesAsync(int offset, int limit)
        {
            return this.context.Categories.Where(c => c.CategoryId >= offset).Take(limit).ToList();
        }

        /// <inheritdoc/>
        public bool TryShowCategoryAsync(int categoryId, out Category productCategory)
        {
            productCategory = this.context.Categories.Find(categoryId);
            return productCategory is not null;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateCategoriesAsync(int categoryId, Category productCategory)
        {
            if (productCategory is null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            var category = this.context.Categories.Single(c => c.CategoryId == categoryId);
            if (category is not null)
            {
                category.CategoryName = productCategory.CategoryName;
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
