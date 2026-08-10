using PortfolioEngine.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Application.DTOs;

public record TechnologyDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string IconClass { get; init; } = string.Empty;
    public Guid TechCategoryId { get; init; }
    public string TechCategoryName { get; init; } = string.Empty;
}

public record CreateTechnologyDto(
    string Name,
    string Description,
    string IconClass,
    Guid TechCategoryId
);

public record UpdateTechnologyDto(
    string Name,
    string Description,
    string IconClass,
    Guid TechCategoryId
);