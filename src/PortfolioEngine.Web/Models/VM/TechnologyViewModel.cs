using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PortfolioEngine.Web.Models.VM;

public class TechnologyViewModel
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be 3-10 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(200, MinimumLength = 10, ErrorMessage = "Description must be 10-200 characters.")]
    public string Description { get; set; } = string.Empty;

    [StringLength(40, ErrorMessage = "Icon class cannot exceed 40 characters.")]
    public string IconClass { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category selection is required.")]
    public Guid TechCategoryId { get; set; }

    public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
}
