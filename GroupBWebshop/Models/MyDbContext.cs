using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBWebshop.Models
{
    internal class MyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;database=FashionFusion;Trusted_connection=True;Trustservercertificate=True;");
        }

        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.Customer> Customers { get; set; }
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.Type> Types { get; set; }
        public DbSet<Models.Supplier> Suppliers { get; set; }
    }
}
