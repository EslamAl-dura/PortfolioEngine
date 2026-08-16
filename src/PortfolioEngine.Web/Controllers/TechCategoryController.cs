using Microsoft.AspNetCore.Mvc;
using PortfolioEngine.Application.Common.Interfaces;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Application.Services.Interfaces;
using PortfolioEngine.Web.Models.VM;

namespace PortfolioEngine.WebUI.Controllers;

public class TechCategoryController : Controller
{
    private readonly ITechCategoryService _techCategoryService;

    public TechCategoryController(ITechCategoryService techCategoryService)
    {
        _techCategoryService = techCategoryService;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _techCategoryService.GetAllCategoriesAsync([t => t.Technologies]);

        var viewModel = new TechCategoryListViewModel
        {
            Categories = categories.ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTechCategoryViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors });
        }
        try
        {
            var dto = new CreateTechCategoryDto(model.Name, model.Description, model.IconClass, model.Type);
            await _techCategoryService.CreateCategoryAsync(dto);

            return Json(new { success = true, message = "Category created successfully!" });
        }
        catch (Exception)
        {
            // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
            // _logger.LogError(ex, "Error creating category");
            return Json(new { success = false, message = "An error occurred while creating the category." });
        }
    }

    // POST: Update Category
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateTechCategoryViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors });
        }
        try
        {
            var dto = new CreateTechCategoryDto(model.Name, model.Description, model.IconClass, model.Type);
            await _techCategoryService.UpdateCategoryAsync(id, dto, cancellationToken); // Ensure Update is in your Service/Interface

            return Json(new { success = true, message = "Category updated successfully!" });
        }
        catch (Exception)
        {
            // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
            // _logger.LogError(ex, "Error updating category");
            return Json(new { success = false, message = "An error occurred while updating the category." });
        }
    }

    // POST: Delete Category
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _techCategoryService.DeleteCategoryAsync(id);
        return Json(new { success = true, message = "Category deleted successfully!" });
    }
}