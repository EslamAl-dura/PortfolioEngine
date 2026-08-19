using System;

namespace PortfolioEngine.Application.DTOs;

public record ColleagueDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string AvatarUrl { get; init; } = string.Empty;
    public string ProfileUrl { get; init; } = string.Empty;
    public bool UntilNow { get; init; }
    public DateTimeOffset StartDate { get; init; }
    public DateTimeOffset EndDate { get; init; }
}

public record CreateColleagueDto(
    string Name,
    string Role,
    string AvatarUrl,
    string ProfileUrl,
    bool UntilNow,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate
);

public record UpdateColleagueDto(
    string Name,
    string Role,
    string AvatarUrl,
    string ProfileUrl,
    bool UntilNow,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate
);
