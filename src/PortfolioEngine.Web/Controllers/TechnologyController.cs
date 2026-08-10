using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Application.Services.Interfaces;
using PortfolioEngine.Web.Models.VM;

namespace PortfolioEngine.Web.Controllers;

public class TechnologyController : Controller
{
    private readonly ITechnologyService _technologyService;
    private readonly ITechCategoryService _techCategoryService;

    public TechnologyController(
        ITechnologyService technologyService,
        ITechCategoryService techCategoryService)
    {
        _technologyService = technologyService;
        _techCategoryService = techCategoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var technologies = await _technologyService.GetAllTechnologiesAsync([t => t.TechCategory!]);

        ViewBag.Categories = await _techCategoryService.GetCategorySelectListAsync();

        return View(technologies);
    }

    [HttpGet]
    public async Task<IActionResult> GetForEdit(Guid id)
    {
        var tech = await _technologyService.GetTechnologyByIdAsync(id);
        if (tech == null) return NotFound();

        return Json(new
        {
            id = tech.Id,
            name = tech.Name,
            description = tech.Description,
            iconClass = tech.IconClass,
            techCategoryId = tech.TechCategoryId
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TechnologyViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors });
        }
        try
        {

            var dto = new CreateTechnologyDto(model.Name, model.Description, model.IconClass, model.TechCategoryId);
            await _technologyService.CreateTechnologyAsync(dto);

            return Json(new { success = true, message = "Technology added successfully!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "An error occurred while adding the technology." });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, TechnologyViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors });
        }
        try
        {

            var dto = new UpdateTechnologyDto(model.Name, model.Description, model.IconClass, model.TechCategoryId);
            await _technologyService.UpdateTechnologyAsync(id, dto);

            return Json(new { success = true, message = "Technology updated successfully!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "An error occurred while updating the technology." });
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _technologyService.DeleteTechnologyAsync(id);
        return Json(new { success = true, message = "Technology deleted successfully!" });
    }


}
