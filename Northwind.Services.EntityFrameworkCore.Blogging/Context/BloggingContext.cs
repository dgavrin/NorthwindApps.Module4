using Microsoft.EntityFrameworkCore;
using Northwind.Services.EntityFrameworkCore.Blogging.Entities;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Context
{
    public class BloggingContext : DbContext
    {
        public DbSet<BlogArticle> Articles { get; set; }

        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogArticle>()
                .HasKey(a => a.BlogArticleId);

            modelBuilder.Entity<BlogArticle>(
                eb =>
                {
                    eb.Property(a => a.BlogArticleId)
                        .HasColumnType("int")
                        .HasColumnName("blog_article_id");
                    eb.Property(a => a.Title)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("title");
                    eb.Property(a => a.Body)
                        .HasColumnType("nvarchar(4000)")
                        .HasColumnName("body");
                    eb.Property(a => a.PublicationDate)
                        .HasColumnType("date")
                        .HasColumnName("publication_date");
                    eb.Property(a => a.EmployeeId)
                        .HasColumnType("int")
                        .HasColumnName("employee_id");
                });
        }
    }
}
