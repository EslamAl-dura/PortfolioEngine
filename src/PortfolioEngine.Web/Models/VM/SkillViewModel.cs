using PortfolioEngine.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace PortfolioEngine.Web.Models.VM;

public class SkillViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Skill name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string IconClass { get; set; } = "lightning-charge-fill";

    [Range(0, 100, ErrorMessage = "Proficiency must be between 0 and 100.")]
    public int Proficiency { get; set; } = 80;

    public bool IsSoftSkill { get; set; } = false;

    public List<Guid> SelectedTechnologyIds { get; set; } = new();
}