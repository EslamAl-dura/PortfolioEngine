using PortfolioEngine.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Application.DTOs;

public record TechCategoryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string IconClass { get; init; } = string.Empty;
    public CategoryTypes Type { get; init; }

    public IReadOnlyCollection<TechnologyDto> Technologies { get; init; } = Array.Empty<TechnologyDto>();
}
public record CreateTechCategoryDto(string Name, string Description, string IconClass, CategoryTypes Type);
public record UpdateTechCategoryDto(Guid Id, string Name, string Description, string IconClass, CategoryTypes Type);
