using PortfolioEngine.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Domain.Entities;

public class Project : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string LiveUrl { get; set; } = string.Empty;
    public string GithubUrl { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool IsFinished { get; set; } = false;
    public bool Featured { get; set; } = false;
    public bool IsLive { get; set; } = false;
    public bool IsPublic { get; set; } = false;
    public bool IsTeamWork { get; set; } = false;
    public DateTimeOffset StartDate { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset EndDate { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<Colleague> Colleagues { get; set; } = new HashSet<Colleague>();
    public ICollection<Skill> Skills { get; set; } = new HashSet<Skill>();
}
