using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
