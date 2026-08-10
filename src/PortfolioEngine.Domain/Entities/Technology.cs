using PortfolioEngine.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace PortfolioEngine.Domain.Entities;

/// <summary>
/// Technology represents a specific technology within a category in the portfolio engine domain.
/// </summary>
public class Technology : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconClass { get; set; } = string.Empty;
    public Guid TechCategoryId { get; set; }
    public TechCategory? TechCategory { get; set; }
}

