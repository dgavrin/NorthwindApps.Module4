using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.Blogging
{
    public class BlogArticle
    {
        [Key]
        [Column("BlogArticleID")]
        public int BlogArticleId { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(4000)]
        public string Body { get; set; }

        [Column(TypeName = "date")]
        public DateTime PublicationDate { get; set; }

        [Column("EmployeeID")]
        public int EmployeeId { get; set; }
    }
}
