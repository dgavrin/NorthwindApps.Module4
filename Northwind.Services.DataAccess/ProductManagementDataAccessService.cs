using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Northwind.DataAccess;
using Northwind.DataAccess.Products;
using Northwind.Services.Products;

namespace Northwind.Services.DataAccess
{
    public class ProductManagementDataAccessService : IProductManagementService
    {
        private NorthwindDataAccessFactory northwindDataAccessFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductManagementDataAccessService"/> class.
        /// </summary>
        /// <param name="sqlConnection">Sql connection.</param>
        public ProductManagementDataAccessService(SqlConnection sqlConnection)
        {
            this.northwindDataAccessFactory = new SqlServerDataAccessFactory(sqlConnection) ?? throw new ArgumentNullException(nameof(sqlConnection));
        }

        /// <inheritdoc/>
        public int CreateProduct(Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            return this.northwindDataAccessFactory.GetProductDataAccessObject().InsertProduct((ProductTransferObject)product);
        }

        /// <inheritdoc/>
        public bool DestroyProduct(int productId)
        {
            if (productId < 1)
            {
                throw new ArgumentException("ProductId can't be less than one.", nameof(productId));
            }

            return this.northwindDataAccessFactory.GetProductDataAccessObject().DeleteProduct(productId);
        }

        /// <inheritdoc/>
        public IList<Product> LookupProductsByName(IList<string> names)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IList<Product> ShowProducts(int offset, int limit)
        {
            var products = new List<Product>();
            foreach (var productTransferObkect in this.northwindDataAccessFactory.GetProductDataAccessObject().SelectProducts(offset, limit))
            {
                products.Add((Product)productTransferObkect);
            }

            return products;
        }

        /// <inheritdoc/>
        public IList<Product> ShowProductsForCategory(int categoryId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryShowProduct(int productId, out Product product)
        {
            if (productId < 1)
            {
                throw new ArgumentException("ProductId can't be less than one.", nameof(productId));
            }

            try
            {
                product = (Product)this.northwindDataAccessFactory.GetProductDataAccessObject().FindProduct(productId);
            }
            catch (ProductNotFoundException)
            {
                product = null;
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public bool UpdateProduct(int productId, Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (productId != product.Id)
            {
                return false;
            }

            if (this.northwindDataAccessFactory.GetProductDataAccessObject().UpdateProduct((ProductTransferObject)product))
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
