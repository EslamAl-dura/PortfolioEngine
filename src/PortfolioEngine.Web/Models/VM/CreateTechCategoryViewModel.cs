using PortfolioEngine.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PortfolioEngine.Web.Models.VM;

public class CreateTechCategoryViewModel
{
    [Required(ErrorMessage = "Category Name is required.")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category Type is required.")]
    public CategoryTypes Type { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(200, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 200 characters.")]
    public string Description { get; set; } = string.Empty;

    [StringLength(40, ErrorMessage = "Icon class cannot exceed 40 characters.")]
    public string IconClass { get; set; } = "bi bi-code-slash";
}