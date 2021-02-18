using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFrameworkCore
{
    /// <summary>
    /// Represents a product management service.
    /// </summary>
    public sealed class ProductManagementService : IProductManagementService
    {
        private readonly NorthwindContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductManagementService"/> class.
        /// </summary>
        /// <param name="context">Northwind context.</param>
        public ProductManagementService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public async Task<int> CreateProductAsync(Product product)
        {
            if (product is null)
            {
                return -1;
            }

            if (this.context.Products.Any())
            {
                product.Id = this.context.Products.Max(p => p.Id) + 1;
            }
            else
            {
                product.Id = 0;
            }

            this.context.Products.Add(product);
            await this.context.SaveChangesAsync();
            return product.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyProductAsync(int productId)
        {
            var product = this.context.Products.Find(productId);
            if (product is not null)
            {
                this.context.Products.Remove(product);
                await this.context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<IList<Product>> LookupProductsByNameAsync(IList<string> names)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<IList<Product>> ShowProductsAsync(int offset, int limit)
        {
            return this.context.Products.Where(p => p.Id >= offset).Take(limit).ToList();
        }

        /// <inheritdoc/>
        public async Task<IList<Product>> ShowProductsForCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryShowProduct(int productId, out Product product)
        {
            product = this.context.Products.Find(productId);
            return product is not null;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateProductAsync(int productId, Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            var newProduct = this.context.Products.Single(c => c.Id == productId);
            if (newProduct is not null)
            {
                newProduct.Name = product.Name;
                newProduct.SupplierId = product.SupplierId;
                newProduct.CategoryId = product.CategoryId;
                newProduct.QuantityPerUnit = product.QuantityPerUnit;
                newProduct.UnitPrice = product.UnitPrice;
                newProduct.UnitsInStock = product.UnitsInStock;
                newProduct.UnitsOnOrder = product.UnitsOnOrder;
                newProduct.ReorderLevel = product.ReorderLevel;
                newProduct.Discontinued = product.Discontinued;
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
