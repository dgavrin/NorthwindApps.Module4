using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;

namespace NorthwindApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
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
        public async Task<ActionResult<Category>> CreateCategoryAsync(Category productCategory)
        {
            if (productCategory is null)
            {
                return this.BadRequest();
            }

            await this.productCategoryManagementService.CreateCategoryAsync(productCategory);
            return this.Ok(productCategory);
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<Category>> GetCategoryAsync(int categoryId)
        {
            if (this.productCategoryManagementService.TryShowCategoryAsync(categoryId, out Category productCategory))
            {
                return this.Ok(productCategory);
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesAsync(int offset = 0, int limit = 10)
        {
            var categories = await this.productCategoryManagementService.ShowCategoriesAsync(offset, limit);
            return this.Ok(categories);
        }

        [HttpPut("{categoryId}")]
        public async Task<ActionResult> UpdateCategoryAsync(int categoryId, Category productCategory)
        {
            if (productCategory is null)
            {
                throw new ArgumentNullException(nameof(productCategory));
            }

            if (categoryId != productCategory.CategoryId)
            {
                return this.BadRequest();
            }

            await this.productCategoryManagementService.UpdateCategoriesAsync(categoryId, productCategory);
            return this.NoContent();
        }

        [HttpDelete("{categoryId}")]
        public async Task<ActionResult<Category>> DeleteCategoryAsync(int categoryId)
        {
            if (await this.productCategoryManagementService.DestroyCategoryAsync(categoryId))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpPut("{categoryId}/picture")]
        public async Task<ActionResult> PutPictureAsync(int categoryId, IFormFile formFile)
        {
            if (categoryId < 1)
            {
                throw new ArgumentException("CategoryId can't be less than one.", nameof(categoryId));
            }

            using var stream = new MemoryStream();
            await formFile?.CopyToAsync(stream);
            if (!(await this.productCategoryPicturesService.UpdatePictureAsync(categoryId, stream)))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        [HttpGet("{categoryId}/picture")]
        public async Task<ActionResult<byte[]>> GetPicture(int categoryId)
        {
            if (this.productCategoryPicturesService.TryShowPicture(categoryId, out byte[] picture))
            {
                return this.Ok(picture);
            }

            return this.NotFound();
        }

        [HttpDelete("{categoryId}/picture")]
        public async Task<ActionResult> DeletePictureAsync(int categoryId)
        {
            if (await this.productCategoryPicturesService.DestroyPictureAsync(categoryId))
            {
                return this.NoContent();
            }

            return this.NotFound();
        }
    }
}
