using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable
#pragma warning disable CS1591
#pragma warning disable SA1601
#pragma warning disable SA1600

namespace Northwind.Services.Products
{
    public partial class Product
    {
        [Key]
        [Column("ProductID")]
        public int ProductId { get; set; }

        [StringLength(50)]
        public string ProductName { get; set; }

        [Column("SupplierID")]
        public int? SupplierId { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(50)]
        public string QuantityPerUnit { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        public decimal? UnitPrice { get; set; }

        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }

        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }
    }
}
#pragma warning restore SA1600
#pragma warning restore SA1601
#pragma warning restore CS1591
