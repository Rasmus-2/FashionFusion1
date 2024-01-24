using Microsoft.EntityFrameworkCore;

namespace GroupBWebshop.Models
{
    internal class MyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //ändra "SQLEXPRESS" om det inte funkar
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;database=NewFashionFusion;Trusted_connection=True;Trustservercertificate=True;");
        }

        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.Customer> Customers { get; set; }
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.Supplier> Suppliers { get; set; }
        public DbSet<Models.Country> Countries { get; set; }
        public DbSet<Models.Order> Orders { get; set; }
        public DbSet<Models.OrderDetails> OrderDetails { get; set; }
    }
}
