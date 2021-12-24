

using Microsoft.EntityFrameworkCore;
using TradingData.Models;

namespace TradingData.DBContext
{
    public class TraderDbContext : DbContext
    {
        public TraderDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Equity> Equities { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use Fluent API to configure  

            // Map entities to tables  
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Orders>().ToTable("orders");
            modelBuilder.Entity<Equity>().ToTable("equity");
        }
    }
    
}
