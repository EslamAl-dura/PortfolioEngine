using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Application.Services.Interfaces;
using PortfolioEngine.Web.Models.VM;

namespace PortfolioEngine.Web.Controllers;

public class SkillController : Controller
{
    private readonly ISkillService _skillService;
    private readonly ITechnologyService _technologyService;

    public SkillController(ISkillService skillService, ITechnologyService technologyService)
    {
        _skillService = skillService;
        _technologyService = technologyService;
    }

    // GET: /Skill
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var skills = await _skillService.GetAllSkillsAsync([s => s.Technologies], cancellationToken);

        // Fetch technologies dropdown/multiselect items
        var technologies = await _technologyService.GetAllTechnologiesAsync(cancellationToken: cancellationToken);
        ViewBag.Technologies = technologies.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name
        }).ToList();

        return View(skills);
    }

    // POST: /Skill/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SkillViewModel model, CancellationToken cancellationToken)
    {
        ModelState.Remove(nameof(model.Id));

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors });
        }

        try
        {
            var dto = new CreateSkillDto(
                model.Name,
                model.Description,
                model.IconClass,
                model.Proficiency,
                model.IsSoftSkill,
                model.SelectedTechnologyIds
            );

            await _skillService.CreateSkillAsync(dto, cancellationToken);
            return Json(new { success = true, message = "Skill created successfully!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"An error occurred while creating the skill.{ex.Message}" });
        }
    }

    // POST: /Skill/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, SkillViewModel model, CancellationToken cancellationToken)
    {
        ModelState.Remove(nameof(model.Id));

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors });
        }

        try
        {
            var dto = new UpdateSkillDto(
                model.Name,
                model.Description,
                model.IconClass,
                model.Proficiency,
                model.IsSoftSkill,
                model.SelectedTechnologyIds
            );

            await _skillService.UpdateSkillAsync(id, dto, cancellationToken);
            return Json(new { success = true, message = "Skill updated successfully!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"An error occurred while updating the skill.- {ex.Message}" });
        }
    }

    // POST: /Skill/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _skillService.DeleteSkillAsync(id, cancellationToken);
            return Json(new { success = true, message = "Skill deleted successfully!" });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = "An error occurred while deleting the skill." });
        }
    }

}
