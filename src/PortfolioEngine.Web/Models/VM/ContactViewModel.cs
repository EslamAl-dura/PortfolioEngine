using PortfolioEngine.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PortfolioEngine.Web.Models.VM;

public class ContactViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Value is required.")]
    [StringLength(200, ErrorMessage = "Value cannot exceed 200 characters.")]
    public string Value { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Icon Class")]
    [StringLength(50, ErrorMessage = "Icon class cannot exceed 50 characters.")]
    public string IconClass { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select a contact type.")]
    [Display(Name = "Contact Type")]
    public ContactTypes ContactType { get; set; } = ContactTypes.Other;
}