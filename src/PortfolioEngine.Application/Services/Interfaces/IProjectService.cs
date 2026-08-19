using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PortfolioEngine.Application.Services.Interfaces;

public interface IProjectService
{
    Task<IReadOnlyList<ProjectDto>> GetAllProjectsAsync(
        Expression<Func<Project, object>>[]? includes = null,
        CancellationToken cancellationToken = default);

    Task<ProjectDto?> GetProjectByIdAsync(
        Guid id,
        Expression<Func<Project, object>>[]? includes = null,
        CancellationToken cancellationToken = default);

    Task<Guid> CreateProjectAsync(CreateProjectDto dto, CancellationToken cancellationToken = default);

    Task UpdateProjectAsync(Guid id, UpdateProjectDto dto, CancellationToken cancellationToken = default);

    Task DeleteProjectAsync(Guid id, CancellationToken cancellationToken = default);
}