using AutoMapper;
using PortfolioEngine.Application.Common.Interfaces;
using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Application.Services.Interfaces;
using PortfolioEngine.Domain.Entities;
using PortfolioEngine.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PortfolioEngine.Application.Services.Implementations;

public class ContactService : IContactService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;
    public ContactService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> CreateContactAsync(CreateContactDto dto, CancellationToken cancellationToken = default)
    {
        var contactRepo = _unitOfWork.Repository<Contact, Guid>();

        var contactEntity = _mapper.Map<Contact>(dto);

        await contactRepo.AddAsync(contactEntity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return contactEntity.Id;
    }

    public async Task DeleteContactAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contactRepo = _unitOfWork.Repository<Contact, Guid>();
        var contact = await contactRepo.GetByIdAsync(id, cancellationToken: cancellationToken);
        if (contact != null)
        {
            contactRepo.Delete(contact);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<ContactDto>> GetAllContactsAsync(Expression<Func<Contact, object>>[]? includes = null, CancellationToken cancellationToken = default)
    {
        var contactRepo = _unitOfWork.Repository<Contact, Guid>();
        var contacts = await contactRepo.GetAllAsync(includes, cancellationToken: cancellationToken);
        return contacts.Select(c => _mapper.Map<ContactDto>(c));
    }

    public async Task<ContactDto> GetContactByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contactRepo = _unitOfWork.Repository<Contact, Guid>();
        var contact = await contactRepo.GetByIdAsync(id, cancellationToken: cancellationToken);
        return contact == null ? new ContactDto() : _mapper.Map<ContactDto>(contact);
    }

    public async Task<IEnumerable<ContactDto>> GetContactByTypeAsync(ContactTypes contactType, CancellationToken cancellationToken = default)
    {
        var contactRepo = _unitOfWork.Repository<Contact, Guid>();
        var contacts = await contactRepo.GetAllAsync(null, cancellationToken: cancellationToken);
        IEnumerable<Contact> filteredContacts = contacts.Where(c => c.ContactType == contactType);
        return filteredContacts == null ? Enumerable.Empty<ContactDto>() : filteredContacts.Select(c => _mapper.Map<ContactDto>(c));
    }

    public async Task UpdateContactAsync(Guid id, UpdateContactDto dto, CancellationToken cancellationToken = default)
    {
        var contactRepo = _unitOfWork.Repository<Contact, Guid>();
        var contact = await contactRepo.GetByIdAsync(id, cancellationToken: cancellationToken);

        if (contact != null)
        {
            _mapper.Map(dto, contact);
            contactRepo.Update(contact);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
