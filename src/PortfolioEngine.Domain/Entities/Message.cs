using PortfolioEngine.Domain.Common;

namespace PortfolioEngine.Domain.Entities;

public class Message : BaseEntity
{
    public string SenderName { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
}
