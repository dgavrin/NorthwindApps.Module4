using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;

namespace NorthwindApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private IProductManagementService productManagementService;

        public ProductCategoriesController(IProductManagementService service)
        {
            this.productManagementService = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpPost]
        public async Task<ActionResult<ProductCategory>> CreateCategory(ProductCategory productCategory)
        {
            if (productCategory is null)
            {
                return this.BadRequest();
            }

            this.productManagementService.CreateCategory(productCategory);
            return this.CreatedAtAction(nameof(this.GetCategory), new { categoryId = productCategory.Id }, productCategory);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategory>> GetCategory(int categoryId)
        {
            if (this.productManagementService.TryShowCategory(categoryId, out ProductCategory productCategory))
            {
                return this.Ok(categoryId);
            }
            else
            {
                return this.NotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetCategories(int offset, int limit)
        {
            return this.Ok(this.productManagementService.ShowCategories(offset, limit));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int categoryId, ProductCategory productCategory)
        {
            if (categoryId != productCategory.Id)
            {
                return this.BadRequest();
            }

            this.productManagementService.UpdateCategories(categoryId, productCategory);
            return this.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductCategory>> DeletCategory(int categoryId)
        {
            if (this.productManagementService.DestroyCategory(categoryId))
            {
                return this.NoContent();
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}
