using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Northwind.DataAccess.Products;
using Northwind.Services.Products;

namespace Northwind.DataAccess.SqlServer.Products
{
    public class ProductCategoryPicturesManagementDataAccessService : IProductCategoryPicturesService
    {
        private NorthwindDataAccessFactory northwindDataAccessFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryPicturesManagementDataAccessService"/> class.
        /// </summary>
        /// <param name="sqlConnection">Sql connection.</param>
        public ProductCategoryPicturesManagementDataAccessService(SqlConnection sqlConnection)
        {
            this.northwindDataAccessFactory = new SqlServerDataAccessFactory(sqlConnection) ?? throw new ArgumentNullException(nameof(sqlConnection));
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyPictureAsync(int categoryId)
        {
            if (categoryId < 1)
            {
                throw new ArgumentException("CategoryId can't be less than one.", nameof(categoryId));
            }

            ProductCategoryTransferObject productCategoryTransferObject;
            try
            {
                productCategoryTransferObject = this.northwindDataAccessFactory.GetProductCategoryDataAccessObject().FindProductCategory(categoryId);
            }
            catch (ProductCategoryNotFoundException)
            {
                return false;
            }

            productCategoryTransferObject.Picture = null;
            if (await this.northwindDataAccessFactory.GetProductCategoryDataAccessObject().UpdateProductCategoryAsync(productCategoryTransferObject))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool TryShowPicture(int categoryId, out byte[] bytes)
        {
            if (categoryId < 1)
            {
                throw new ArgumentException("CategoryId can't be less than one.", nameof(categoryId));
            }

            ProductCategoryTransferObject productCategoryTransferObject;
            try
            {
                productCategoryTransferObject = this.northwindDataAccessFactory.GetProductCategoryDataAccessObject().FindProductCategory(categoryId);
            }
            catch (ProductCategoryNotFoundException)
            {
                bytes = null;
                return false;
            }

            bytes = productCategoryTransferObject.Picture;
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdatePictureAsync(int categoryId, Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (categoryId < 1)
            {
                throw new ArgumentException("CategoryId can't be less than one.", nameof(categoryId));
            }

            ProductCategoryTransferObject productCategoryTransferObject;

            try
            {
                productCategoryTransferObject = this.northwindDataAccessFactory.GetProductCategoryDataAccessObject().FindProductCategory(categoryId);
            }
            catch (ProductCategoryNotFoundException)
            {
                return false;
            }

            using var memoryStream = new MemoryStream();
            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(memoryStream);
            productCategoryTransferObject.Picture = memoryStream.ToArray();

            if (await this.northwindDataAccessFactory.GetProductCategoryDataAccessObject().UpdateProductCategoryAsync(productCategoryTransferObject))
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
