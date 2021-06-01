using System.Collections.Generic;
using DotNet.Core.Data.EF.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNet.Core.Data.EF.Domain.EntitiesConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.ToTable("Product");

            entity.Property(e => e.Id).HasColumnName("Id");

            entity.HasData(new List<Product>()
            {
                new Product()
                {
                    Description = "A description",
                    Name = "Product A",
                    Price = 49.99
                },
                new Product()
                {
                    Description = "B description",
                    Name = "Product B",
                    Price = 29.99
                },
                new Product()
                {
                    Description = "C description",
                    Name = "Product C",
                    Price = 89.99
                }
            });
        }
    }
}