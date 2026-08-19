using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace PortfolioEngine.Web.Models.VM;

public class ColleagueViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Colleague name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role/Designation is required.")]
    [StringLength(100, ErrorMessage = "Role cannot exceed 100 characters.")]
    public string Role { get; set; } = string.Empty;

    public string? AvatarUrl { get; set; }

    [Url(ErrorMessage = "Please enter a valid URL.")]
    public string? ProfileUrl { get; set; }

    public bool UntilNow { get; set; }

    [Required(ErrorMessage = "Start date is required.")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    public IFormFile? AvatarFile { get; set; }
}
