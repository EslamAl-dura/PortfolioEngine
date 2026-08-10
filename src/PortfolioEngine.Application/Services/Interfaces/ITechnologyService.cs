using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Domain.Entities;

namespace PortfolioEngine.Application.Services.Interfaces;

public interface ITechnologyService
{
    Task<IEnumerable<TechnologyDto>> GetAllTechnologiesAsync(Expression<Func<Technology, object>>[]? includes = null, CancellationToken cancellationToken = default);
    Task<TechnologyDto?> GetTechnologyByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateTechnologyAsync(CreateTechnologyDto dto, CancellationToken cancellationToken = default);
    Task UpdateTechnologyAsync(Guid id, UpdateTechnologyDto dto, CancellationToken cancellationToken = default);
    Task DeleteTechnologyAsync(Guid id, CancellationToken cancellationToken = default);
}