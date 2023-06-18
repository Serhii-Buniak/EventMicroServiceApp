using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CityMicroService.DAL.RepositoryBase;

public interface IRepositoryBase<T>
{
    Func<IQueryable<T>, IIncludableQueryable<T, object>>? Include { get; set; }
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
    void Create(T entity);
    Task CreateAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    void Attach(T entity);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null);
    Task<Tuple<IEnumerable<T>, int>> GetRangeAsync(Expression<Func<T, bool>>? filter = null,
                                                           Expression<Func<T, T>>? selector = null,
                                                           Func<IQueryable<T>, IQueryable<T>>? sorting = null,
                                                           int? pageNumber = null,
                                                           int? pageSize = null);
    Task<T> SingleAsync(Expression<Func<T, bool>>? predicate = null);
    Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>>? predicate = null);
}
