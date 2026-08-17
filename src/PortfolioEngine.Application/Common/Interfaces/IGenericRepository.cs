using PortfolioEngine.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
namespace PortfolioEngine.Application.Common.Interfaces;

public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdWithIncludesAsync(TKey id, Expression<Func<TEntity, object>>[]? includes = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> GetAllAsync(Expression<Func<TEntity, object>>[]? includes = null,CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, bool track = false, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity); // Performs soft delete via AuditInterceptor
}