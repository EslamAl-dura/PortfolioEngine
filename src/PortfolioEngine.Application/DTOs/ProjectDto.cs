using PortfolioEngine.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PortfolioEngine.Application.DTOs;

public record ProjectDto
{
    public Guid Id { get; set; }
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

    // Nested lookup DTOs instead of raw domain entities
    public IReadOnlyCollection<ColleagueLookupDto> Colleagues { get; init; } = Array.Empty<ColleagueLookupDto>();
    public IReadOnlyCollection<SkillLookupDto> Skills { get; init; } = Array.Empty<SkillLookupDto>();
}
public record ColleagueLookupDto(Guid Id, string Name, string? Role);
public record SkillLookupDto(Guid Id, string Name, string IconClass);


public record CreateProjectDto
{
    [Required(ErrorMessage = "Project title is required.")]
    [StringLength(150, ErrorMessage = "Title cannot exceed 150 characters.")]
    public string Title { get; init; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
    public string Description { get; init; } = string.Empty;

    [Url(ErrorMessage = "Invalid Live URL format.")]
    public string LiveUrl { get; init; } = string.Empty;

    [Url(ErrorMessage = "Invalid GitHub URL format.")]
    public string GithubUrl { get; init; } = string.Empty;

    [StringLength(150)]
    public string Slug { get; init; } = string.Empty;

    public bool IsFinished { get; init; } = false;
    public bool Featured { get; init; } = false;
    public bool IsLive { get; init; } = false;
    public bool IsPublic { get; init; } = true;
    public bool IsTeamWork { get; init; } = false;

    [Required]
    public DateTimeOffset StartDate { get; init; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? EndDate { get; init; }

    // Foreign Keys for Relationships
    public List<Guid> ColleagueIds { get; init; } = new();
    public List<Guid> SkillIds { get; init; } = new();
}


public record UpdateProjectDto
{
    [Required]
    public Guid Id { get; init; }

    [Required(ErrorMessage = "Project title is required.")]
    [StringLength(150)]
    public string Title { get; init; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(2000)]
    public string Description { get; init; } = string.Empty;

    [Url]
    public string LiveUrl { get; init; } = string.Empty;

    [Url]
    public string GithubUrl { get; init; } = string.Empty;

    [StringLength(150)]
    public string Slug { get; init; } = string.Empty;

    public bool IsFinished { get; init; }
    public bool Featured { get; init; }
    public bool IsLive { get; init; }
    public bool IsPublic { get; init; }
    public bool IsTeamWork { get; init; }

    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset? EndDate { get; init; }

    // Updated Foreign Keys
    public List<Guid> ColleagueIds { get; init; } = new();
    public List<Guid> SkillIds { get; init; } = new();
}