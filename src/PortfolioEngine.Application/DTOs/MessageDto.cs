using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioEngine.Application.DTOs;

public class MessageDto
{
    public Guid Id { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class MessageFilterDto
{
    public string? Search { get; set; }
    public bool? IsRead { get; set; }
    public string SortBy { get; set; } = "CreatedAt";
    public bool IsDescending { get; set; } = true;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
