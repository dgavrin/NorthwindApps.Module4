using System;
using System.Collections.Generic;
using System.Linq;
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
        public int CreateProduct(Product product)
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
            this.context.SaveChanges();
            return product.Id;
        }

        /// <inheritdoc/>
        public bool DestroyProduct(int productId)
        {
            var product = this.context.Products.Find(productId);
            if (product is not null)
            {
                this.context.Products.Remove(product);
                this.context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public IList<Product> LookupProductsByName(IList<string> names)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IList<Product> ShowProducts(int offset, int limit)
        {
            return this.context.Products.Where(p => p.Id >= offset).Take(limit).ToList();
        }

        /// <inheritdoc/>
        public IList<Product> ShowProductsForCategory(int categoryId)
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
        public bool UpdateProduct(int productId, Product product)
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
