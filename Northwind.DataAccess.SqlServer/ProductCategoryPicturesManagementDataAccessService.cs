using System;
using System.Data.SqlClient;
using System.IO;
using Northwind.DataAccess.Products;
using Northwind.Services.Products;

namespace Northwind.DataAccess.SqlServer
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
        public bool DestroyPicture(int categoryId)
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
            if (this.northwindDataAccessFactory.GetProductCategoryDataAccessObject().UpdateProductCategory(productCategoryTransferObject))
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
        public bool UpdatePicture(int categoryId, Stream stream)
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
            stream.CopyTo(memoryStream);
            productCategoryTransferObject.Picture = memoryStream.ToArray();

            if (this.northwindDataAccessFactory.GetProductCategoryDataAccessObject().UpdateProductCategory(productCategoryTransferObject))
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
