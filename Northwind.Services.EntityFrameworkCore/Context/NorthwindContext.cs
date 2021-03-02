using Microsoft.EntityFrameworkCore;
using Northwind.Services.Employees;
using Northwind.Services.Products;

#nullable disable
#pragma warning disable CA1062
#pragma warning disable CS1591
#pragma warning disable SA1600
#pragma warning disable SA1601

namespace Northwind.Services.EntityFrameworkCore.Context
{
    public partial class NorthwindContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NorthwindContext"/> class.
        /// </summary>
        public NorthwindContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NorthwindContext"/> class.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public NorthwindContext(DbContextOptions<NorthwindContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
#pragma warning restore SA1601
#pragma warning restore SA1600
#pragma warning restore CS1591
#pragma warning restore CA1062
