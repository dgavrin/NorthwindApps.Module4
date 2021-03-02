using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Products;

namespace NorthwindApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ProductsController : Controller
    {
        private IProductManagementService productManagementService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="productManagementService">IProductManagementService.</param>
        public ProductsController(IProductManagementService productManagementService)
        {
            this.productManagementService = productManagementService ?? throw new ArgumentNullException(nameof(productManagementService));
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProductAsync(Product product)
        {
            if (product is null)
            {
                return this.BadRequest();
            }

            await this.productManagementService.CreateProductAsync(product);
            return this.Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsAsync(int offset = 0, int limit = 10)
        {
            if (offset >= 0 && limit > 0)
            {
                return this.Ok(await this.productManagementService.ShowProductsAsync(offset, limit));
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpGet("{productId}")]
        public ActionResult<Product> GetProduct(int productId)
        {
            if (this.productManagementService.TryShowProduct(productId, out Product product))
            {
                return this.Ok(product);
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult> UpdateProductAsync(int productId, Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (productId != product.ProductId)
            {
                return this.BadRequest();
            }

            await this.productManagementService.UpdateProductAsync(productId, product);
            return this.NoContent();
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult<Product>> DeleteProductAsync(int productId)
        {
            if (await this.productManagementService.DestroyProductAsync(productId))
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
