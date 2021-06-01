using DotNet.Core.Data.EF.Domain.Entities;
using DotNet.Core.Data.EF.Persistence;
using DotNet.Core.Data.EF.Repositories.Interfaces;

namespace DotNet.Core.Data.EF.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
  {
    public ProductRepository(ApplicationContext dbContext)
      : base(dbContext)
    {
    }

  }
}
