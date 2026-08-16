using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Application.Services.Interfaces;
using PortfolioEngine.Domain.Enums;
using PortfolioEngine.Web.Models.VM;

namespace PortfolioEngine.Web.Controllers;

public class ContactController : Controller
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    // GET: /Contact
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var contacts = await _contactService.GetAllContactsAsync(null,cancellationToken);
        ViewBag.ContactTypes = GetContactTypeSelectList();
        return View(contacts);
    }

    // POST: /Contact/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContactViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors });
        }

        try
        {
            var dto = new CreateContactDto(model.Name, model.Value, model.Description, model.IconClass, model.ContactType);
            await _contactService.CreateContactAsync(dto, cancellationToken);

            return Json(new { success = true, message = "Contact added successfully!" });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = "An error occurred while creating the contact." });
        }
    }

    // POST: /Contact/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ContactViewModel model, CancellationToken cancellationToken)
    {
        // Ignore Id validation errors on the model since we take `id` from the route
        ModelState.Remove(nameof(model.Id));

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors });
        }

        try
        {
            var dto = new UpdateContactDto(model.Name, model.Value, model.Description, model.IconClass, model.ContactType);
            await _contactService.UpdateContactAsync(id, dto, cancellationToken);

            return Json(new { success = true, message = "Contact updated successfully!" });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = "An error occurred while updating the contact." });
        }
    }

    // POST: /Contact/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _contactService.DeleteContactAsync(id, cancellationToken);
            return Json(new { success = true, message = "Contact deleted successfully!" });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = "An error occurred while deleting the contact." });
        }
    }

    private static SelectList GetContactTypeSelectList(ContactTypes? selected = null)
    {
        var values = Enum.GetValues(typeof(ContactTypes))
            .Cast<ContactTypes>()
            .Select(e => new { Id = e, Name = e.ToString() });

        return new SelectList(values, "Id", "Name", selected);
    }
}