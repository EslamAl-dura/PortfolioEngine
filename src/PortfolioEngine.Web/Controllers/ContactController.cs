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
        return View(contacts);
    }

    // GET: /Contact/Create (Modal Partial)
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.ContactTypes = GetContactTypeSelectList();
        return PartialView("_CreateModal", new ContactViewModel());
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

    // GET: /Contact/Edit/{id} (Modal Partial)
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var contact = await _contactService.GetContactByIdAsync(id, cancellationToken);
        if (contact == null || contact.Id == Guid.Empty)
        {
            return NotFound();
        }

        var model = new ContactViewModel
        {
            Id = contact.Id,
            Name = contact.Name,
            Value = contact.Value,
            Description = contact.Description,
            IconClass = contact.IconClass,
            ContactType = contact.ContactType
        };

        ViewBag.ContactTypes = GetContactTypeSelectList(contact.ContactType);
        return PartialView("_EditModal", model);
    }

    // POST: /Contact/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ContactViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, errors });
        }

        try
        {
            var dto = new UpdateContactDto(model.Name, model.Value, model.Description, model.IconClass, model.ContactType);
            await _contactService.UpdateContactAsync(model.Id, dto, cancellationToken);

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