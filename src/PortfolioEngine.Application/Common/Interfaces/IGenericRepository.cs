using PortfolioEngine.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
namespace PortfolioEngine.Application.Common.Interfaces;

public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    // Reads
    Task<IReadOnlyList<TEntity>> GetAllAsync(
        Expression<Func<TEntity, object>>[]? includes = null,
        bool track = false,
        bool withIdentityResolution = false,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(
        TKey id,
        bool track = false,
        bool withIdentityResolution = false,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default);

    // ... same pattern for GetByIdWithIncludesAsync, FindAsync, FirstOrDefaultAsync
    Task<TEntity?> GetByIdWithIncludesAsync(
        TKey id,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool track = false,
        bool withIdentityResolution = false,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool track = false,
        bool withIdentityResolution = false,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default);

    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool track = false,
        bool withIdentityResolution = false,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default);

    // Projections
    Task<IReadOnlyList<TResult>> GetAllProjectedAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default);

    Task<TResult?> GetByIdProjectedAsync<TResult>(
        TKey id,
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TResult>> FindProjectedAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default);

    // Pagination
    Task<PagedResult<TEntity>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool ascending = true,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool track = false,
        bool withIdentityResolution = false,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default);
    Task<PagedResult<TResult>> GetPagedProjectedAsync<TResult>(
        int pageNumber,
        int pageSize,
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool ascending = true,
        Expression<Func<TEntity, object>>[]? includes = null,
        bool ignoreQueryFilters = false,
        CancellationToken cancellationToken = default);

    // Writes
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}