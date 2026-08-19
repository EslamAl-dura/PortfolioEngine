using Microsoft.AspNetCore.Mvc;
using PortfolioEngine.Application.Common.Interfaces;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Application.Services.Interfaces;
using PortfolioEngine.Web.Models.VM;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PortfolioEngine.Web.Controllers;

public class ColleagueController : Controller
{
    private readonly IColleagueService _colleagueService;
    private readonly IFileStorageService _fileStorageService;

    public ColleagueController(IColleagueService colleagueService, IFileStorageService fileStorageService)
    {
        _colleagueService = colleagueService;
        _fileStorageService = fileStorageService;
    }

    // GET: /Colleague
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var colleagues = await _colleagueService.GetAllColleaguesAsync(cancellationToken: cancellationToken);
        return View(colleagues);
    }

    // POST: /Colleague/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ColleagueViewModel model, CancellationToken cancellationToken)
    {
        ModelState.Remove(nameof(model.Id));
        ModelState.Remove(nameof(model.AvatarUrl));

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors });
        }

        try
        {
            string avatarUrl = string.Empty;
            if (model.AvatarFile != null && model.AvatarFile.Length > 0)
            {
                avatarUrl = await _fileStorageService.UploadFileAsync(model.AvatarFile, "colleagues", cancellationToken);
            }

            var dto = new CreateColleagueDto(
                model.Name,
                model.Role,
                avatarUrl,
                model.ProfileUrl ?? string.Empty,
                model.UntilNow,
                new DateTimeOffset(model.StartDate, TimeSpan.Zero),
                model.UntilNow ? DateTimeOffset.UtcNow : new DateTimeOffset(model.EndDate ?? DateTime.UtcNow, TimeSpan.Zero)
            );

            await _colleagueService.CreateColleagueAsync(dto, cancellationToken);
            return Json(new { success = true, message = "Colleague created successfully!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"An error occurred while creating the colleague. {ex.Message}" });
        }
    }

    // POST: /Colleague/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ColleagueViewModel model, CancellationToken cancellationToken)
    {
        ModelState.Remove(nameof(model.Id));
        ModelState.Remove(nameof(model.AvatarUrl));

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors });
        }

        try
        {
            var existing = await _colleagueService.GetColleagueByIdAsync(id, cancellationToken: cancellationToken);
            if (existing == null)
            {
                return Json(new { success = false, message = "Colleague not found." });
            }

            string avatarUrl = existing.AvatarUrl;
            if (model.AvatarFile != null && model.AvatarFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(avatarUrl))
                {
                    await _fileStorageService.DeleteFileAsync(avatarUrl);
                }
                avatarUrl = await _fileStorageService.UploadFileAsync(model.AvatarFile, "colleagues", cancellationToken);
            }

            var dto = new UpdateColleagueDto(
                model.Name,
                model.Role,
                avatarUrl,
                model.ProfileUrl ?? string.Empty,
                model.UntilNow,
                new DateTimeOffset(model.StartDate, TimeSpan.Zero),
                model.UntilNow ? DateTimeOffset.UtcNow : new DateTimeOffset(model.EndDate ?? DateTime.UtcNow, TimeSpan.Zero)
            );

            await _colleagueService.UpdateColleagueAsync(id, dto, cancellationToken);
            return Json(new { success = true, message = "Colleague updated successfully!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"An error occurred while updating the colleague. {ex.Message}" });
        }
    }

    // POST: /Colleague/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var existing = await _colleagueService.GetColleagueByIdAsync(id, cancellationToken: cancellationToken);
            if (existing != null)
            {
                if (!string.IsNullOrEmpty(existing.AvatarUrl))
                {
                    await _fileStorageService.DeleteFileAsync(existing.AvatarUrl);
                }
                await _colleagueService.DeleteColleagueAsync(id, cancellationToken);
            }
            return Json(new { success = true, message = "Colleague deleted successfully!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"An error occurred while deleting the colleague. {ex.Message}" });
        }
    }
}
