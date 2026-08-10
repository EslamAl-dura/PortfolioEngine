using PortfolioEngine.Domain.Common;
using PortfolioEngine.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PortfolioEngine.Domain.Entities;

/// <summary>
/// TechCategory represents a category of technologies in the portfolio engine domain.
/// </summary>
public class TechCategory : BaseEntity
{
    
    public string Name { get; set; } = string.Empty;
    public CategoryTypes Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public string IconClass { get; set; } = string.Empty;
    public ICollection<Technology> Technologies { get; set; } = new List<Technology>();
}
