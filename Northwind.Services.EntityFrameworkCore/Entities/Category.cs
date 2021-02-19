using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable
#pragma warning disable CS1591
#pragma warning disable SA1601
#pragma warning disable SA1600
#pragma warning disable CA1819

namespace Northwind.Services.EntityFrameworkCore.Entities
{
    public partial class Category
    {
        [Key]
        [Column("CategoryID")]
        public int CategoryId { get; set; }

        [StringLength(50)]
        public string CategoryName { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public byte[] Picture { get; set; }
    }
}
#pragma warning restore CA1819
#pragma warning restore SA1600
#pragma warning restore SA1601
#pragma warning restore CS1591
