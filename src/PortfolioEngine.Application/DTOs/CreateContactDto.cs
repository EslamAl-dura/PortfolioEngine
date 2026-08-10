using PortfolioEngine.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Application.DTOs;

public record ContactDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string IconClass { get; init; } = string.Empty;
    public ContactTypes ContactType { get; init; }
}

public record CreateContactDto(string Name, string Value, string Description, string IconClass, ContactTypes ContactType);
public record UpdateContactDto(string Name, string Value, string Description, string IconClass, ContactTypes ContactType);