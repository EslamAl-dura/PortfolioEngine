using PortfolioEngine.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PortfolioEngine.Domain.Entities;

public class Skill : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconClass { get; set; } = string.Empty;
    [Range(0, 100)]
    public int Proficiency { get; set; } = 0;
    public bool IsSoftSkill { get; set; } = false;

    public ICollection<Technology> Technologies { get; set; } = new HashSet<Technology>();
}
