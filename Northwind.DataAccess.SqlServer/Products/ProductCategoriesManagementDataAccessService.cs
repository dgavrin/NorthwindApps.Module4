using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Northwind.DataAccess.Products;
using Northwind.Services.Products;

namespace Northwind.DataAccess.SqlServer.Products
{
    public class ProductCategoriesManagementDataAccessService : IProductCategoryManagementService
    {
        private NorthwindDataAccessFactory northwindDataAccessFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoriesManagementDataAccessService"/> class.
        /// </summary>
        /// <param name="sqlConnection">Sql connection.</param>
        public ProductCategoriesManagementDataAccessService(SqlConnection sqlConnection)
        {
            this.northwindDataAccessFactory = new SqlServerDataAccessFactory(sqlConnection) ?? throw new ArgumentNullException(nameof(sqlConnection));
        }

        /// <inheritdoc/>
        public int CreateCategory(ProductCategory productCategory)
        {
            if (productCategory is null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            return this.northwindDataAccessFactory.GetProductCategoryDataAccessObject().InsertProductCategory((ProductCategoryTransferObject)productCategory);
        }

        /// <inheritdoc/>
        public bool DestroyCategory(int categoryId)
        {
            if (categoryId < 1)
            {
                throw new ArgumentException("CategoryId can't be less than one.", nameof(categoryId));
            }

            return this.northwindDataAccessFactory.GetProductCategoryDataAccessObject().DeleteProductCategory(categoryId);
        }

        /// <inheritdoc/>
        public IList<ProductCategory> LookupCategoriesByName(IList<string> names)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IList<ProductCategory> ShowCategories(int offset, int limit)
        {
            var productCategories = new List<ProductCategory>();
            foreach (var productTransferObkect in this.northwindDataAccessFactory.GetProductCategoryDataAccessObject().SelectProductCategories(offset, limit))
            {
                productCategories.Add((ProductCategory)productTransferObkect);
            }

            return productCategories;
        }

        /// <inheritdoc/>
        public bool TryShowCategory(int categoryId, out ProductCategory productCategory)
        {
            if (categoryId < 1)
            {
                throw new ArgumentException("CategoryId can't be less than one.", nameof(categoryId));
            }

            try
            {
                productCategory = (ProductCategory)this.northwindDataAccessFactory.GetProductCategoryDataAccessObject().FindProductCategory(categoryId);
            }
            catch (ProductNotFoundException)
            {
                productCategory = null;
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public bool UpdateCategories(int categoryId, ProductCategory productCategory)
        {
            if (productCategory is null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            if (categoryId != productCategory.Id)
            {
                return false;
            }

            if (this.northwindDataAccessFactory.GetProductCategoryDataAccessObject().UpdateProductCategory((ProductCategoryTransferObject)productCategory))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
