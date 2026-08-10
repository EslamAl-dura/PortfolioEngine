using PortfolioEngine.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Application.Common.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}