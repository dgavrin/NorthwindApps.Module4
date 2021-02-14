using System;
using System.Collections.Generic;
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

        public ProductsController(IProductManagementService productManagementService)
        {
            this.productManagementService = productManagementService ?? throw new ArgumentNullException(nameof(productManagementService));
        }

        [HttpPost]
        public ActionResult<Product> CreateProduct(Product product)
        {
            if (product is null)
            {
                return this.BadRequest();
            }

            this.productManagementService.CreateProduct(product);
            return this.Ok(product);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts(int offset = 0, int limit = 10)
        {
            if (offset >= 0 && limit > 0)
            {
                return this.Ok(this.productManagementService.ShowProducts(offset, limit));
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
        public ActionResult UpdateProduct(int productId, Product product)
        {
            if (productId != product.Id)
            {
                return this.BadRequest();
            }

            this.productManagementService.UpdateProduct(productId, product);
            return this.NoContent();
        }

        [HttpDelete("{productId}")]
        public ActionResult<Product> DeleteProduct(int productId)
        {
            if (this.productManagementService.DestroyProduct(productId))
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
