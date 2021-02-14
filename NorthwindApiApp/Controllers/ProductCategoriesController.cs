using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;

namespace NorthwindApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private IProductCategoryManagementService productCategoryManagementService;
        private IProductCategoryPicturesService productCategoryPicturesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoriesController"/> class.
        /// </summary>
        /// <param name="productCategoryManagementService">Product category management service.</param>
        /// <param name="productCategoryPicturesService">Product category picture service.</param>
        public ProductCategoriesController(IProductCategoryManagementService productCategoryManagementService, IProductCategoryPicturesService productCategoryPicturesService)
        {
            this.productCategoryManagementService = productCategoryManagementService ?? throw new ArgumentNullException(nameof(productCategoryManagementService));
            this.productCategoryPicturesService = productCategoryPicturesService ?? throw new ArgumentNullException(nameof(productCategoryPicturesService));
        }

        [HttpPost]
        public ActionResult<ProductCategory> CreateCategory(ProductCategory productCategory)
        {
            if (productCategory is null)
            {
                return this.BadRequest();
            }

            this.productCategoryManagementService.CreateCategory(productCategory);
            return this.Ok(productCategory);
        }

        [HttpGet("{categoryId}")]
        public ActionResult<ProductCategory> GetCategory(int categoryId)
        {
            if (this.productCategoryManagementService.TryShowCategory(categoryId, out ProductCategory productCategory))
            {
                return this.Ok(productCategory);
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductCategory>> GetCategories(int offset = 0, int limit = 10)
        {
            return this.Ok(this.productCategoryManagementService.ShowCategories(offset, limit));
        }

        [HttpPut("{categoryId}")]
        public ActionResult UpdateCategory(int categoryId, ProductCategory productCategory)
        {
            if (categoryId != productCategory.Id)
            {
                return this.BadRequest();
            }

            this.productCategoryManagementService.UpdateCategories(categoryId, productCategory);
            return this.NoContent();
        }

        [HttpDelete("{categoryId}")]
        public ActionResult<ProductCategory> DeleteCategory(int categoryId)
        {
            if (this.productCategoryManagementService.DestroyCategory(categoryId))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpPut("{categoryId}/picture")]
        public ActionResult PutPicture(int categoryId, IFormFile formFile)
        {
            if (categoryId < 1)
            {
                throw new ArgumentException("CategoryId can't be less than one.", nameof(categoryId));
            }

            using var stream = new MemoryStream();
            formFile?.CopyTo(stream);
            if (!this.productCategoryPicturesService.UpdatePicture(categoryId, stream))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        [HttpGet("{categoryId}/picture")]
        public ActionResult<byte[]> GetPicture(int categoryId)
        {
            if (this.productCategoryPicturesService.TryShowPicture(categoryId, out byte[] picture))
            {
                return this.Ok(picture);
            }

            return this.NotFound();
        }

        [HttpDelete("{categoryId}/picture")]
        public ActionResult DeletePicture(int categoryId)
        {
            if (this.productCategoryPicturesService.DestroyPicture(categoryId))
            {
                return this.NoContent();
            }

            return this.NotFound();
        }
    }
}
