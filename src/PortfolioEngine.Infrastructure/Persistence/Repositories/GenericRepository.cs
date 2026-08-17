using Microsoft.EntityFrameworkCore;
using PortfolioEngine.Application.Common.Interfaces;
using PortfolioEngine.Domain.Common;
using System.Linq.Expressions;

namespace PortfolioEngine.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    protected readonly ApplicationDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _dbSet = _dbContext.Set<TEntity>();
    }

    // ---------- READ ----------

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(
        Expression<Func<TEntity, object>>[]? includes = null,
        bool track = false,
        bool withIdentityResolution = false,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default)
    {
        return await BuildQuery(track, withIdentityResolution, ignoreQueryFilters, includes)
            .ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(
        TKey id,
        bool track = false,
        bool withIdentityResolution = false,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default)
    {
        return await BuildQuery(track, withIdentityResolution, ignoreQueryFilters)
            .FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
    }

    public async Task<TEntity?> GetByIdWithIncludesAsync(
        TKey id,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool track = false,
        bool withIdentityResolution = false,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default)
    {
        return await BuildQuery(track, withIdentityResolution, ignoreQueryFilters, includes)
            .FirstOrDefaultAsync(e => e.Id!.Equals(id), cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool track = false,
        bool withIdentityResolution = false,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default)
    {
        return await BuildQuery(track, withIdentityResolution, ignoreQueryFilters, includes)
            .Where(predicate)
            .ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool track = false,
        bool withIdentityResolution = false,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default)
    {
        return await BuildQuery(track, withIdentityResolution, ignoreQueryFilters, includes)
            .FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();
        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        return await query.AnyAsync(predicate, cancellationToken);
    }

    public async Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();
        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.CountAsync(cancellationToken);
    }

    // ---------- PROJECTIONS ----------

    public async Task<IReadOnlyList<TResult>> GetAllProjectedAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default)
    {
        return await BuildQuery(track: false, withIdentityResolution: false, ignoreQueryFilters, includes)
            .Select(selector)
            .ToListAsync(cancellationToken);
    }

    public async Task<TResult?> GetByIdProjectedAsync<TResult>(
        TKey id,
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default)
    {
        return await BuildQuery(track: false, withIdentityResolution: false, ignoreQueryFilters, includes)
            .Where(e => e.Id!.Equals(id))
            .Select(selector)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TResult>> FindProjectedAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default)
    {
        return await BuildQuery(track: false, withIdentityResolution: false, ignoreQueryFilters, includes)
            .Where(predicate)
            .Select(selector)
            .ToListAsync(cancellationToken);
    }

    // ---------- PAGINATION ----------

    public async Task<PagedResult<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool ascending = true,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool track = false,
        bool withIdentityResolution = false,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default)
    {
        ValidatePagination(pageNumber, pageSize);

        IQueryable<TEntity> query = BuildQuery(track, withIdentityResolution, ignoreQueryFilters, includes);

        if (predicate is not null)
            query = query.Where(predicate);

        var totalCount = await query.CountAsync(cancellationToken);

        if (orderBy is not null)
        {
            query = ascending
                ? query.OrderBy(orderBy)
                : query.OrderByDescending(orderBy);
        }

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<TEntity>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<PagedResult<TResult>> GetPagedProjectedAsync<TResult>(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool ascending = true,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default)
    {
        ValidatePagination(pageNumber, pageSize);

        IQueryable<TEntity> query = BuildQuery(
            track: false,
            withIdentityResolution: false,
            ignoreQueryFilters,
            includes);

        if (predicate is not null)
            query = query.Where(predicate);

        var totalCount = await query.CountAsync(cancellationToken);

        if (orderBy is not null)
        {
            query = ascending
                ? query.OrderBy(orderBy)
                : query.OrderByDescending(orderBy);
        }

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(selector)
            .ToListAsync(cancellationToken);

        return new PagedResult<TResult>(items, totalCount, pageNumber, pageSize);
    }

    // ---------- WRITE ----------

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Add(entity);
        return Task.CompletedTask;
    }

    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);
        _dbSet.AddRange(entities);
        return Task.CompletedTask;
    }

    public void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        // Interceptor converts Remove → Soft Delete
        _dbSet.Remove(entity);
    }

    // ---------- Private helpers ----------

    private IQueryable<TEntity> BuildQuery(
        bool track,
        bool withIdentityResolution,
        bool ignoreQueryFilters,
        Expression<Func<TEntity, object>>[]? includes = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (ignoreQueryFilters)
            query = query.IgnoreQueryFilters();

        if (!track)
        {
            query = withIdentityResolution
                ? query.AsNoTrackingWithIdentityResolution()
                : query.AsNoTracking();
        }

        if (includes is { Length: > 0 })
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query;
    }

    private static void ValidatePagination(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be >= 1.");
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be >= 1.");
    }
}