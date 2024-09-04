using Microsoft.EntityFrameworkCore;
using RecyclingHub.Models.DbModels;
namespace RecyclingHub.Data
{
    public class HubContext(DbContextOptions<HubContext> opts) : DbContext(opts)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Insection> Insections { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Aquisition> Acquisitions { get; set; }
        public DbSet<Disposal> Disposals { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>()
                .Navigation(i => i.Product)
                .AutoInclude();
        }
        
    }
}
