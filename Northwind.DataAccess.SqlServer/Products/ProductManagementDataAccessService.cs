using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Northwind.DataAccess.Products;
using Northwind.Services.Products;

namespace Northwind.DataAccess.SqlServer.Products
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
        public async Task<int> CreateProductAsync(Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            return await this.northwindDataAccessFactory.GetProductDataAccessObject().InsertProductAsync((ProductTransferObject)product);
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyProductAsync(int productId)
        {
            if (productId < 1)
            {
                throw new ArgumentException("ProductId can't be less than one.", nameof(productId));
            }

            return await this.northwindDataAccessFactory.GetProductDataAccessObject().DeleteProductAsync(productId);
        }

        /// <inheritdoc/>
        public async Task<IList<Product>> LookupProductsByNameAsync(IList<string> names)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<IList<Product>> ShowProductsAsync(int offset, int limit)
        {
            var products = new List<Product>();
            foreach (var productTransferObkect in await this.northwindDataAccessFactory.GetProductDataAccessObject().SelectProductsAsync(offset, limit))
            {
                products.Add((Product)productTransferObkect);
            }

            return products;
        }

        /// <inheritdoc/>
        public async Task<IList<Product>> ShowProductsForCategoryAsync(int categoryId)
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
        public async Task<bool> UpdateProductAsync(int productId, Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (productId != product.Id)
            {
                return false;
            }

            if (await this.northwindDataAccessFactory.GetProductDataAccessObject().UpdateProductAsync((ProductTransferObject)product))
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
