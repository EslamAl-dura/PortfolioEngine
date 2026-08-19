using Microsoft.AspNetCore.Mvc;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Application.Services.Interfaces;

namespace PortfolioEngine.Web.Controllers;

public class MessageController : Controller
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    // GET: /Message
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] MessageFilterDto filter, CancellationToken cancellationToken)
    {
        var pagedData = await _messageService.GetPagedMessagesAsync(filter, cancellationToken);
        ViewBag.Filter = filter;
        return View(pagedData);
    }

    // GET: /Message/Details/{id}
    [HttpGet]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        var message = await _messageService.GetMessageByIdAsync(id, cancellationToken);
        if (message == null) return NotFound();

        return Json(new { success = true, data = message });
    }

    // POST: /Message/ToggleRead/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleRead(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _messageService.ToggleReadStatusAsync(id, cancellationToken);
            return Json(new { success = true, message = "Status updated successfully." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    // POST: /Message/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _messageService.DeleteMessageAsync(id, cancellationToken);
            return Json(new { success = true, message = "Message deleted successfully." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
}