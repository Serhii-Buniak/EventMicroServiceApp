using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CityMicroService.DAL.RepositoryBase;


public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected ApplicationDbContext AppDbContext { get; set; }
    protected DbSet<T> DbSet { get; set; }

    public RepositoryBase(ApplicationDbContext appDbContext)
    {
        AppDbContext = appDbContext;
        DbSet = appDbContext.Set<T>();
    }

    public Func<IQueryable<T>, IIncludableQueryable<T, object>>? Include { get; set; } = null;

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return DbSet.Where(expression).AsNoTracking();
    }

    public void Create(T entity)
    {
        DbSet.Add(entity);
    }

    public async Task CreateAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        DbSet.Remove(entity);
    }

    public void Attach(T entity)
    {
        DbSet.Attach(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
    {
        return await GetQuery(predicate).ToListAsync();
    }

    public async Task<Tuple<IEnumerable<T>, int>> GetRangeAsync(Expression<Func<T, bool>>? filter = null,
                                                           Expression<Func<T, T>>? selector = null,
                                                           Func<IQueryable<T>, IQueryable<T>>? sorting = null,
                                                           int? pageNumber = null,
                                                           int? pageSize = null)
    {
        return await GetRangeQuery(filter, selector, sorting, pageNumber, pageSize);
    }

    public async Task<T> SingleAsync(Expression<Func<T, bool>>? predicate = null)
    {
        var query = GetQuery(predicate);
        if (Include != null)
        {
            query = Include(query);
        }
        return await query.SingleAsync();
    }

    public async Task<T?> SingleOrDefaultAsync(Expression<Func<T, bool>>? predicate = null)
    {
        return await GetQuery(predicate).SingleOrDefaultAsync();
    }

    private IQueryable<T> GetQuery(Expression<Func<T, bool>>? predicate = null)
    {
        var query = DbSet.AsNoTracking();
        if (Include != null)
        {
            query = Include(query);
        }
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        return query;
    }
    private async Task<Tuple<IEnumerable<T>, int>> GetRangeQuery(Expression<Func<T, bool>>? filter = null,
                                                      Expression<Func<T, T>>? selector = null,
                                                      Func<IQueryable<T>, IQueryable<T>>? sorting = null,
                                                      int? pageNumber = null,
                                                      int? pageSize = null)
    {
        var query = DbSet.AsNoTracking();

        if (Include != null)
        {
            query = Include(query);
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (selector != null)
        {
            query = query.Select(selector);
        }

        if (sorting != null)
        {
            query = sorting(query);
        }

        var TotalRecords = await query.CountAsync();

        if (pageNumber != null && pageSize != null)
        {
            query = query.Skip((int)(pageSize * (pageNumber - 1)))
                .Take((int)pageSize);
        }

        return new Tuple<IEnumerable<T>, int>(query, TotalRecords);
    }
}