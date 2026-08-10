using Microsoft.AspNetCore.Identity;

namespace PortfolioEngine.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    // Custom domain properties
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}