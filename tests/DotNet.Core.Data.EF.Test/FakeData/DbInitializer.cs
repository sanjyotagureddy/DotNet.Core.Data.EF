using System;
using System.Collections.Generic;
using DotNet.Core.Data.EF.Domain.Entities;
using DotNet.Core.Data.EF.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DotNet.Core.Data.EF.Test.FakeData
{
    public static class DbInitializer
    {
        public static ApplicationContext CreateFakeDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
              .Options;

            var context = new ApplicationContext(options);
            SeedDatabase(context);
            return context;
        }

        private static void SeedDatabase(ApplicationContext context)
        {
            context.Products.AddRangeAsync(GetListOfProducts());
            context.SaveChangesAsync();
        }

        private static IEnumerable<Product> GetListOfProducts()
        {
            return new List<Product>()
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
      };
        }
    }
}
