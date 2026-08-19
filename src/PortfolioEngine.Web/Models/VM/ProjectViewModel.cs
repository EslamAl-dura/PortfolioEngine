using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortfolioEngine.Web.Models.VM;

public class ProjectViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    public string LiveUrl { get; set; } = string.Empty;
    public string GithubUrl { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public bool IsFinished { get; set; } = false;
    public bool Featured { get; set; } = false;
    public bool IsLive { get; set; } = false;
    public bool IsPublic { get; set; } = true;
    public bool IsTeamWork { get; set; } = false;

    [Required(ErrorMessage = "Start Date is required.")]
    public DateTime StartDate { get; set; } = DateTime.Today;

    public DateTime? EndDate { get; set; }

    public List<Guid> SelectedColleagueIds { get; set; } = new();
    public List<Guid> SelectedSkillIds { get; set; } = new();
}