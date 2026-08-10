using PortfolioEngine.Application.DTOs;
using PortfolioEngine.Domain.Entities;
using PortfolioEngine.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PortfolioEngine.Application.Services.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<ContactDto>> GetAllContactsAsync(Expression<Func<Contact, object>>[]? includes = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<ContactDto>> GetContactByTypeAsync(ContactTypes contactType, CancellationToken cancellationToken = default);
        Task<ContactDto> GetContactByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Guid> CreateContactAsync(CreateContactDto dto, CancellationToken cancellationToken = default);
        Task UpdateContactAsync(Guid id, UpdateContactDto dto, CancellationToken cancellationToken = default);
        Task DeleteContactAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
