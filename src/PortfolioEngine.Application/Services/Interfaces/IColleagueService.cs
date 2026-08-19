using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace PortfolioEngine.Application.Services.Interfaces;

public interface IColleagueService
{
    Task<IReadOnlyList<ColleagueDto>> GetAllColleaguesAsync(
        Expression<Func<Colleague, object>>[]? includes = null,
        CancellationToken cancellationToken = default);

    Task<ColleagueDto?> GetColleagueByIdAsync(
        Guid id,
        Expression<Func<Colleague, object>>[]? includes = null,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateColleagueAsync(
        CreateColleagueDto dto,
        CancellationToken cancellationToken = default);

    Task UpdateColleagueAsync(
        Guid id,
        UpdateColleagueDto dto,
        CancellationToken cancellationToken = default);

    Task DeleteColleagueAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
