using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Config
{
    public class ProductConfigration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
            builder.Property(a=>a.Price).HasColumnType("decimal");
            builder.HasOne(a=>a.ProductBrand).WithMany().HasForeignKey(a=>a.ProductBrandId);
            builder.HasOne(a=>a.ProductType).WithMany().HasForeignKey(a=>a.ProductTypeId);

        }
    }
}
