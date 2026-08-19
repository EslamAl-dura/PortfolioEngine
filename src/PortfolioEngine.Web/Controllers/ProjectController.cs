using Microsoft.AspNetCore.Mvc;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Application.Services.Interfaces;
using PortfolioEngine.Web.Models.VM;

namespace PortfolioEngine.Web.Controllers;

public class ProjectController : Controller
{
    private readonly IProjectService _projectService;
    private readonly IColleagueService _colleagueService;
    private readonly ISkillService _skillService; // Assuming ISkillService exists for options

    public ProjectController(
        IProjectService projectService,
        IColleagueService colleagueService,
        ISkillService skillService)
    {
        _projectService = projectService;
        _colleagueService = colleagueService;
        _skillService = skillService;
    }

    // GET: /Project
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var projects = await _projectService.GetAllProjectsAsync(cancellationToken: cancellationToken);

        ViewBag.Colleagues = await _colleagueService.GetAllColleaguesAsync(cancellationToken: cancellationToken);
        ViewBag.Skills = await _skillService.GetAllSkillsAsync(cancellationToken: cancellationToken);

        return View(projects);
    }

    // POST: /Project/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProjectViewModel model, CancellationToken cancellationToken)
    {
        ModelState.Remove(nameof(model.Id));

        if (!model.IsFinished && model.EndDate.HasValue && model.EndDate < model.StartDate)
        {
            ModelState.AddModelError("EndDate", "End date cannot be earlier than start date.");
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors });
        }

        try
        {
            var dto = new CreateProjectDto
            {
                Title = model.Title,
                Description = model.Description,
                LiveUrl = model.LiveUrl ?? string.Empty,
                GithubUrl = model.GithubUrl ?? string.Empty,
                Slug = model.Slug ?? string.Empty,
                IsFinished = model.IsFinished,
                Featured = model.Featured,
                IsLive = model.IsLive,
                IsPublic = model.IsPublic,
                IsTeamWork = model.IsTeamWork,
                StartDate = new DateTimeOffset(model.StartDate, TimeSpan.Zero),
                EndDate = model.IsFinished && model.EndDate.HasValue
                    ? new DateTimeOffset(model.EndDate.Value, TimeSpan.Zero)
                    : null,
                ColleagueIds = model.SelectedColleagueIds,
                SkillIds = model.SelectedSkillIds
            };

            await _projectService.CreateProjectAsync(dto, cancellationToken);
            return Json(new { success = true, message = "Project created successfully!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
        }
    }

    // POST: /Project/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ProjectViewModel model, CancellationToken cancellationToken)
    {
        ModelState.Remove(nameof(model.Id));

        if (!model.IsFinished && model.EndDate.HasValue && model.EndDate < model.StartDate)
        {
            ModelState.AddModelError("EndDate", "End date cannot be earlier than start date.");
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors });
        }

        try
        {
            var existing = await _projectService.GetProjectByIdAsync(id, cancellationToken: cancellationToken);
            if (existing == null)
            {
                return Json(new { success = false, message = "Project not found." });
            }

            var dto = new UpdateProjectDto
            {
                Id = id,
                Title = model.Title,
                Description = model.Description,
                LiveUrl = model.LiveUrl ?? string.Empty,
                GithubUrl = model.GithubUrl ?? string.Empty,
                Slug = model.Slug ?? string.Empty,
                IsFinished = model.IsFinished,
                Featured = model.Featured,
                IsLive = model.IsLive,
                IsPublic = model.IsPublic,
                IsTeamWork = model.IsTeamWork,
                StartDate = new DateTimeOffset(model.StartDate, TimeSpan.Zero),
                EndDate = model.IsFinished && model.EndDate.HasValue
                    ? new DateTimeOffset(model.EndDate.Value, TimeSpan.Zero)
                    : null,
                ColleagueIds = model.SelectedColleagueIds,
                SkillIds = model.SelectedSkillIds
            };

            await _projectService.UpdateProjectAsync(id, dto, cancellationToken);
            return Json(new { success = true, message = "Project updated successfully!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
        }
    }

    // POST: /Project/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _projectService.DeleteProjectAsync(id, cancellationToken);
            return Json(new { success = true, message = "Project deleted successfully!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"An error occurred while deleting: {ex.Message}" });
        }
    }
}
