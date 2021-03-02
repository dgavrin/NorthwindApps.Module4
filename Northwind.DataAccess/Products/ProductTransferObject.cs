using System;
using System.Diagnostics;
using Northwind.Services.Products;

namespace Northwind.DataAccess.Products
{
    /// <summary>
    /// Represents a TO for Northwind products.
    /// </summary>
    [DebuggerDisplay("Id={Id}, Name={Name}")]
    public sealed class ProductTransferObject
    {
        /// <summary>
        /// Gets or sets a product identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a product name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a supplier identifier.
        /// </summary>
        public int? SupplierId { get; set; }

        /// <summary>
        /// Gets or sets a category identifier.
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets a quantity per unit.
        /// </summary>
        public string QuantityPerUnit { get; set; }

        /// <summary>
        /// Gets or sets a unit price.
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets an amount of units in stock.
        /// </summary>
        public short? UnitsInStock { get; set; }

        /// <summary>
        /// Gets or sets an amount of units on order.
        /// </summary>
        public short? UnitsOnOrder { get; set; }

        /// <summary>
        /// Gets or sets a reorder level.
        /// </summary>
        public short? ReorderLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a product is discontinued.
        /// </summary>
        public bool Discontinued { get; set; }

        public static explicit operator ProductTransferObject(Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            ProductTransferObject productTransferObject = new ProductTransferObject()
            {
                Id = product.ProductId,
                Name = product.ProductName,
                SupplierId = product.SupplierId,
                CategoryId = product.CategoryId,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                UnitsOnOrder = product.UnitsOnOrder,
                ReorderLevel = product.ReorderLevel,
                Discontinued = product.Discontinued,
            };

            return productTransferObject;
        }

        public static explicit operator Product(ProductTransferObject productTransferObject)
        {
            if (productTransferObject is null)
            {
                throw new ArgumentNullException(nameof(productTransferObject));
            }

            Product product = new Product()
            {
                ProductId = productTransferObject.Id,
                ProductName = productTransferObject.Name,
                SupplierId = productTransferObject.SupplierId,
                CategoryId = productTransferObject.CategoryId,
                QuantityPerUnit = productTransferObject.QuantityPerUnit,
                UnitPrice = productTransferObject.UnitPrice,
                UnitsInStock = productTransferObject.UnitsInStock,
                UnitsOnOrder = productTransferObject.UnitsOnOrder,
                ReorderLevel = productTransferObject.ReorderLevel,
                Discontinued = productTransferObject.Discontinued,
            };

            return product;
        }
    }
}
