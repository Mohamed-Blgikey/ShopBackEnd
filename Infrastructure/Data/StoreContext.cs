
using Core.Entities;
using Core.Entities.OrderAggregate;
using Infrastructure.Extend;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pipelines.Sockets.Unofficial.Arenas;
using System.Reflection;
using System.Text.Json;

namespace Infrastructure.Data
{
    public class StoreContext : IdentityDbContext<AppUser>
    {

        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

       
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductType> ProductTypes  { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<BasktItem> BasktItems { get; set; }
        public DbSet<Order> Orders  { get; set; }
        public DbSet<OrderItem> OrderItems  { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods   { get; set; }
        public DbSet<OrderAddress> OrderAddresses   { get; set; }
        public DbSet<ProductItemOrdered>  ProductItemOrdereds   { get; set; }
       
    }
}
