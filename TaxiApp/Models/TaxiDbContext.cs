using Microsoft.EntityFrameworkCore;

namespace TaxiApp.Models
{
    public class TaxiDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=taxi.db");
        }
    }
}