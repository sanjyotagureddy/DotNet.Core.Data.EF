using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DotNet.Core.Data.EF.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
  {
    Task<T> GetByIdAsync(int id);
    Task<T> GetByLongIdAsync(long id);
    Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

    Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate,
      Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
      Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
      Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
      bool disableTracking = true);

    Task<T> AddAsync(T entity);
    Task<IReadOnlyList<T>> AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<int> DeleteAsync(Expression<Func<T, bool>> criteria);
  }
}
