using PortfolioEngine.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Domain.Entities;

public class Colleague : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string ProfileUrl { get; set; } = string.Empty;
    public bool UntilNow { get; set; } = false;
    public DateTimeOffset StartDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset EndDate { get; set; } = DateTimeOffset.UtcNow;
}
