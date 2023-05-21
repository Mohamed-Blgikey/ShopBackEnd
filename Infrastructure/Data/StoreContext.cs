
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes  { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var EntityType in modelBuilder.Model.GetEntityTypes())
                {
                    var prop = EntityType.ClrType.GetProperties().Where(a => a.PropertyType == typeof(decimal));
                    foreach (var item in prop)
                    {
                        modelBuilder.Entity(EntityType.Name).Property(item.Name).HasConversion<double>();
                    }
                }
            }
        }
    }
}
