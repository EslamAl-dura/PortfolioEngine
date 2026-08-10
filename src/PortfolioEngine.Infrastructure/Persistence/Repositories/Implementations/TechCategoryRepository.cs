using PortfolioEngine.Application.Common.Interfaces;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Application.Services.Interfaces;
using PortfolioEngine.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using PortfolioEngine.Infrastructure.Persistence.Repositories.Interfaces;

namespace PortfolioEngine.Infrastructure.Persistence.Repositories.Implementations;

public class TechCategoryRepository : IGenericRepository<TechCategory, Guid>, ITechCategoryRepository
{
    protected readonly ApplicationDbContext _context;

    public TechCategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TechCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.TechCategories.FindAsync(new object[] { id }, cancellationToken).AsTask();
        return entity;
    }

    public Task<IReadOnlyList<TechCategory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<TechCategory>> FindAsync(Expression<Func<TechCategory, bool>> predicate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(TechCategory entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Update(TechCategory entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(TechCategory entity)
    {
        throw new NotImplementedException();
    }
}

