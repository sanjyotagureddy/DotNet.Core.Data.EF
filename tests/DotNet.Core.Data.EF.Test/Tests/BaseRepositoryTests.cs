using System.Collections.Generic;
using System.Linq;
using DotNet.Core.Data.EF.Domain.Entities;
using DotNet.Core.Data.EF.Persistence;
using DotNet.Core.Data.EF.Repositories;
using DotNet.Core.Data.EF.Repositories.Interfaces;
using DotNet.Core.Data.EF.Test.FakeData;
using Shouldly;
using Xbehave;

namespace DotNet.Core.Data.EF.Test.Tests
{
  /// <summary>
  /// Unit Tests for base Repository<T>
  /// ProductRepository used as an entry point for testing purpose only 
  /// </summary>
  public class BaseRepositoryTests
  {
    private readonly ApplicationContext _context;
    private readonly IProductRepository _repository;
    public BaseRepositoryTests()
    {

      _context = DbInitializer.CreateFakeDatabase();
      _repository = new ProductRepository(_context);
    }


    #region GET Tests

    [Scenario]
    public void GetAllAsyncReturnsAllProducts()
    {

      "Assertion"
        .x(async () =>
        {
          var result = await _repository.GetAllAsync();
          result.Count.ShouldBe(3);
        });
    }

    [Scenario]
    public void GetByIdAsyncProductExists()
    {
      "Assertion"
        .x(async () =>
        {
          var result = await _repository.GetByIdAsync(1);
          result.ShouldNotBeNull();
          result.Id.ShouldBe(1);
        });
    }

    [Scenario]
    public void GetByIdProductNotExists()
    {
      "Assertion"
        .x(async () =>
        {
          var result = await _repository.GetByIdAsync(5);
          result.ShouldBeNull();
        });
    }

    [Scenario]
    public void GetAsyncWithPredicateProductExists()
    {
      "Assertion"
        .x(async () =>
        {
          var result = await _repository.GetAsync(x => x.Price == 49.99);
          result.ShouldNotBeNull();
          result.Count.ShouldBe(1);
        });
    }

    [Scenario]
    public void GetAsyncWithPredicateProductNotExists()
    {
      "Assertion"
        .x(async () =>
        {
          var result = await _repository.GetAsync(x => x.Price == 9876);
          result.ShouldNotBeNull();
          result.Count.ShouldBe(0);
        });
    }

    [Scenario]
    public void GetFirstOrDefaultAsyncPredicateProductExists()
    {
      "Assertion"
        .x(async () =>
        {
          var result = await _repository.GetFirstOrDefaultAsync(x => x.Name == "Product A");
          result.ShouldNotBeNull();
        });
    }

    [Scenario]
    public void GetFirstOrDefaultAsyncPredicateProductNotExists()
    {
      "Assertion"
        .x(async () =>
        {
          var result = await _repository.GetFirstOrDefaultAsync(x => x.Name == "xyz132");
          result.ShouldBeNull();
        });
    }

    #endregion

    #region ADD Tests

    [Scenario]
    public void AddAsyncProduct()
    {
      Product product = null;
      "Act"
        .x(async () =>
        {
          product = await _repository.AddAsync(GetMockProductToAdd());
          await _context.SaveChangesAsync();
        });

      "Assertion"
        .x(() =>
       {
         product.ShouldNotBeNull();
         product.Id.ShouldBe(4);
       });
    }

    [Scenario]
    public void AddRangeAsyncProducts()
    {
      "Act"
        .x(async () =>
        {
          await _repository.AddRangeAsync(GetMockProductListToAdd());
          await _context.SaveChangesAsync();
        });

      "Assertion"
        .x(async () =>
        {
          var result = await _repository.GetAllAsync();
          result.ShouldNotBeNull();
          result.Count.ShouldBe(5);
        });
    }

    #endregion

    #region UPDATE Tests

    [Scenario]
    public void UpdateAsyncProduct()
    {
      Product product = null;
      "Arrange".x(async () =>
      {
        product = await _repository.GetByIdAsync(1);
        product.Name = "Product X";
        product.Price = 157.2;
      });

      "Act"
      .x(async () =>
      {
        await _repository.UpdateAsync(product);
        await _context.SaveChangesAsync();

      });

      "Assertion"
        .x(async () =>
        {
          var result = await _repository.GetByIdAsync(1);
          result.Name.ShouldBe("Product X");
          result.Price.ShouldBe(157.2);

        });
    }

    #endregion

    #region DELETE Tests

    [Scenario]
    public void DeleteAsyncProductExists()
    {
      "Act"
        .x(async () =>
        {
          var product = await _repository.GetByIdAsync(1);
          await _repository.DeleteAsync(product);
          await _context.SaveChangesAsync();
        });

      "Assertion"
        .x(async () =>
        {
          var result = await _repository.GetByIdAsync(1);
          result.ShouldBeNull();
        });
    }

    [Scenario]
    public void DeleteAsyncProductNotExists()
    {
      "Assertion"
        .x(async () =>
        {
          var product = await _repository.GetByIdAsync(50);
          product.ShouldBeNull();
        });
    }


    #endregion


    #region Private Methods

    private Product GetMockProductToAdd() =>
        new Product()
        {
            Id = 4,
            Description = "D description",
            Name = "Product D",
            Price = 49.99
        };

    private IEnumerable<Product> GetMockProductListToAdd() =>
      new List<Product>()
      {
          new Product()
          {
              Id = 5,
              Description = "E description",
              Name = "Product E",
              Price = 49.99
          },
          new Product()
          {
              Id = 6,
              Description = "F description",
              Name = "Product F",
              Price = 29.99
          }
      }.ToList();

    #endregion
  }
}
