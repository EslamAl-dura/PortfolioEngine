using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Application.DTOs;

public record SkillDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } =string.Empty;
    public string Description { get; init; } = string.Empty;
    public string IconClass { get; init; } = string.Empty;
    public int Proficiency { get; init; }
    public bool IsSoftSkill { get; init; }
}

public record CreateSkillDto ( string Name, string Description, string IconClass, int Proficiency, bool IsSoftSkill );
public record UpdateSkillDto(Guid Id, string Name, string Description, string IconClass, int Proficiency, bool IsSoftSkill);