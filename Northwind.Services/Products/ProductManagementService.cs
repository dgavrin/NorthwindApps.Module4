using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Northwind.Services.Data;

namespace Northwind.Services.Products
{
    /// <summary>
    /// Represents a stub for a product management service.
    /// </summary>
    public sealed class ProductManagementService : IProductManagementService
    {
        private readonly NorthwindContext context;

        public ProductManagementService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public int CreateCategory(ProductCategory productCategory)
        {
            if (productCategory is null)
            {
                return 0;
            }

            this.context.ProductCategories.Add(productCategory);
            this.context.SaveChanges();
            return productCategory.Id;
        }

        /// <inheritdoc/>
        public int CreateProduct(Product product)
        {
            if (product is null)
            {
                return 0;
            }

            this.context.Products.Add(product);
            this.context.SaveChanges();
            return product.Id;
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
        public bool DestroyPicture(int categoryId)
        {
            throw new NotImplementedException();
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
        public IList<ProductCategory> LookupCategoriesByName(IList<string> names)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IList<Product> LookupProductsByName(IList<string> names)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IList<ProductCategory> ShowCategories(int offset, int limit)
        {
            return this.context.ProductCategories.Where(c => c.Id >= offset).Take(limit).ToList();
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
        public bool TryShowCategory(int categoryId, out ProductCategory productCategory)
        {
            productCategory = this.context.ProductCategories.Find(categoryId);
            return productCategory is not null;
        }

        /// <inheritdoc/>
        public bool TryShowPicture(int categoryId, out byte[] bytes)
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

        /// <inheritdoc/>
        public bool UpdatePicture(int categoryId, Stream stream)
        {
            throw new NotImplementedException();
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
