using Microsoft.EntityFrameworkCore;
using Northwind.Services.Blogging;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Context
{
    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options)
            : base(options)
        {
        }

        public DbSet<BlogArticle> Articles { get; set; }

        public DbSet<BlogArticleProduct> ArticleProduct { get; set; }

        public DbSet<BlogComment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogArticle>()
                .HasKey(a => a.BlogArticleId);
            modelBuilder.Entity<BlogArticleProduct>()
                .HasKey(ap => ap.BlogArticleProductId);
            modelBuilder.Entity<BlogComment>()
                .HasKey(c => c.BlogCommentId);

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

            modelBuilder.Entity<BlogArticleProduct>(
                ap =>
                {
                    ap.Property(p => p.BlogArticleProductId)
                        .HasColumnType("int")
                        .HasColumnName("blog_article_product_id");
                    ap.Property(p => p.ArticleId)
                        .HasColumnType("int")
                        .HasColumnName("article_id");
                    ap.Property(p => p.ProductId)
                        .HasColumnType("int")
                        .HasColumnName("product_id");
                });

            modelBuilder.Entity<BlogComment>(
                c =>
                {
                    c.Property(p => p.BlogCommentId)
                        .HasColumnType("int")
                        .HasColumnName("blog_comment_id");
                    c.Property(p => p.ArticleId)
                        .HasColumnType("int")
                        .HasColumnName("article_id");
                    c.Property(p => p.Text)
                        .HasColumnType("nvarchar(500)")
                        .HasColumnName("text");
                    c.Property(p => p.CustomerId)
                        .HasColumnType("int")
                        .HasColumnName("customer_id");
                });
        }
    }
}
