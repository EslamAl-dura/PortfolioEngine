using PortfolioEngine.Application.DTOs;

namespace PortfolioEngine.Web.Models.VM;

public class TechCategoryListViewModel
{
    public string PageTitle { get; set; } = "Technology Stack & Categories";
    public IReadOnlyCollection<TechCategoryDto> Categories { get; set; } = Array.Empty<TechCategoryDto>();

    // UI Helpers
    public bool HasCategories => Categories.Any();
    public int TotalTechnologies => Categories.Sum(c => c.Technologies.Count);
}