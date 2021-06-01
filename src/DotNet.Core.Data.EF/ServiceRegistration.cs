using DotNet.Core.Data.EF.Persistence;
using DotNet.Core.Data.EF.Repositories;
using DotNet.Core.Data.EF.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNet.Core.Data.EF
{
  public static class ServiceRegistration
  {
    public static IServiceCollection AddWmsCommonDataServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<ApplicationContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("ProductsDb")));

      services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

      services.AddScoped<IProductRepository, ProductRepository>();

      return services;
    }
  }
}
